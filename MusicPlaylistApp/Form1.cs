using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WMPLib;

namespace MusicPlaylistApp
{
    public partial class Form1 : Form
    {
        private readonly string _dbPath = Path.Combine(Application.StartupPath, "playlist.db");
        private readonly string _connStr;

        // Windows Media Player (headless)
        private readonly WindowsMediaPlayer _player = new WindowsMediaPlayer();

        private enum RepeatMode { None = 0, One = 1, All = 2 }
        private RepeatMode _repeat = RepeatMode.None;
        private bool _shuffle = false;

        private readonly Random _rng = new Random();
        private List<int> _shuffledOrder = new List<int>();
        private int _currentIndex = -1; // index into listPlaylist.Items

        public Form1()
        {
            InitializeComponent();
            _connStr = $"Data Source={_dbPath};Version=3;";

            // Form events
            this.Load += Form1_Load;

            // Button events
            btnAdd.Click += btnAdd_Click;
            btnRemove.Click += btnRemove_Click;
            btnPlay.Click += btnPlay_Click;
            btnPause.Click += (s, e) => _player.controls.pause();
            btnStop.Click += (s, e) => { _player.controls.stop(); lblNowPlaying.Text = "—"; };
            btnNext.Click += (s, e) => PlayNext();
            btnPrev.Click += (s, e) => PlayPrev();

            // Other UI events
            chkShuffle.CheckedChanged += (s, e) =>
            {
                _shuffle = chkShuffle.Checked;
                RebuildShuffle();
            };
            cmbRepeat.SelectedIndexChanged += (s, e) =>
            {
                _repeat = (RepeatMode)cmbRepeat.SelectedIndex; // 0=None, 1=One, 2=All
            };
            listPlaylist.DoubleClick += (s, e) => PlaySelected();
            listPlaylist.SelectedIndexChanged += (s, e) =>
            {
                _currentIndex = listPlaylist.SelectedIndices.Count > 0 ? listPlaylist.SelectedIndices[0] : -1;
            };

            // Progress
            trackBarProgress.Scroll += trackBarProgress_Scroll;
            timerProgress.Tick += timerProgress_Tick;

            // WMP events
            _player.PlayStateChange += player_PlayStateChange;

            // Basic player config
            _player.settings.autoStart = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup list columns (Title, Artist, Path)
            if (listPlaylist.Columns.Count == 0)
            {
                listPlaylist.Columns.Add("Title", 220);
                listPlaylist.Columns.Add("Artist", 160);
                listPlaylist.Columns.Add("Path", 480);
            }

            if (cmbRepeat.Items.Count == 0)
            {
                cmbRepeat.Items.AddRange(new object[] { "None", "One", "All" });
            }
            cmbRepeat.SelectedIndex = 0;

            EnsureDatabase();
            LoadSongs();
            RebuildShuffle();
        }

        #region Database
        private void EnsureDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Songs(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Artist TEXT,
    FilePath TEXT NOT NULL UNIQUE
);";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadSongs()
        {
            listPlaylist.Items.Clear();

            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Title, Artist, FilePath FROM Songs ORDER BY Id;";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var title = rdr.GetString(0);
                            var artist = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                            var path = rdr.GetString(2);

                            var li = new ListViewItem(title);
                            li.SubItems.Add(artist);
                            li.SubItems.Add(path);
                            listPlaylist.Items.Add(li);
                        }
                    }
                }
            }
        }

        private void AddSongsToDb(IEnumerable<(string Title, string Artist, string Path)> songs)
        {
            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT OR IGNORE INTO Songs(Title, Artist, FilePath) VALUES (@t, @a, @p);";
                    var pTitle = cmd.CreateParameter(); pTitle.ParameterName = "@t";
                    var pArtist = cmd.CreateParameter(); pArtist.ParameterName = "@a";
                    var pPath = cmd.CreateParameter(); pPath.ParameterName = "@p";
                    cmd.Parameters.Add(pTitle);
                    cmd.Parameters.Add(pArtist);
                    cmd.Parameters.Add(pPath);

                    foreach (var s in songs)
                    {
                        pTitle.Value = s.Title;
                        pArtist.Value = string.IsNullOrWhiteSpace(s.Artist) ? DBNull.Value : (object)s.Artist;
                        pPath.Value = s.Path;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        private void RemoveSongFromDb(string filePath)
        {
            using (var conn = new SQLiteConnection(_connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Songs WHERE FilePath=@p;";
                    cmd.Parameters.AddWithValue("@p", filePath);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region UI handlers
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;

            var newRows = new List<(string Title, string Artist, string Path)>();
            foreach (var path in openFileDialog1.FileNames)
            {
                if (!File.Exists(path)) continue;
                var title = Path.GetFileNameWithoutExtension(path);
                var artist = ""; // future: parse ID3 tags
                newRows.Add((title, artist, path));
            }

            if (newRows.Count == 0) return;

            AddSongsToDb(newRows);
            LoadSongs();
            RebuildShuffle();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listPlaylist.SelectedItems.Count == 0) return;

            var li = listPlaylist.SelectedItems[0];
            var path = li.SubItems[2].Text;

            bool wasPlaying = IsCurrentFile(path);

            RemoveSongFromDb(path);
            li.Remove();

            if (wasPlaying)
            {
                _player.controls.stop();
                lblNowPlaying.Text = "—";
            }

            RebuildShuffle();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (listPlaylist.SelectedItems.Count == 0)
            {
                if (listPlaylist.Items.Count == 0) return;
                listPlaylist.Items[0].Selected = true;
            }
            PlaySelected();
        }
        #endregion

        #region Playback
        private void PlaySelected()
        {
            if (listPlaylist.SelectedItems.Count == 0) return;

            var li = listPlaylist.SelectedItems[0];
            _currentIndex = li.Index;

            var path = li.SubItems[2].Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("File not found:\n" + path, "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _player.URL = path;
            _player.controls.play();

            var title = li.Text;
            var artist = li.SubItems.Count > 1 ? li.SubItems[1].Text : "";
            lblNowPlaying.Text = string.IsNullOrWhiteSpace(artist)
                ? $"Now Playing: {title}"
                : $"Now Playing: {title} — {artist}";
        }

        private void player_PlayStateChange(int newState)
        {
            var state = (WMPPlayState)newState;
            if (state == WMPPlayState.wmppsMediaEnded)
            {
                if (_repeat == RepeatMode.One)
                {
                    _player.controls.currentPosition = 0;
                    _player.controls.play();
                }
                else
                {
                    PlayNext();
                }
            }
        }

        private void PlayNext()
        {
            if (listPlaylist.Items.Count == 0) return;

            int nextIndex = -1;

            if (_shuffle)
            {
                if (_shuffledOrder.Count == 0) RebuildShuffle();

                if (_currentIndex < 0)
                {
                    nextIndex = _shuffledOrder[0];
                }
                else
                {
                    int pos = _shuffledOrder.IndexOf(_currentIndex);
                    if (pos >= 0 && pos + 1 < _shuffledOrder.Count)
                    {
                        nextIndex = _shuffledOrder[pos + 1];
                    }
                    else
                    {
                        if (_repeat == RepeatMode.All)
                        {
                            RebuildShuffle();
                            nextIndex = _shuffledOrder[0];
                        }
                        else return;
                    }
                }
            }
            else
            {
                if (_currentIndex < 0) nextIndex = 0;
                else if (_currentIndex + 1 < listPlaylist.Items.Count) nextIndex = _currentIndex + 1;
                else
                {
                    if (_repeat == RepeatMode.All) nextIndex = 0;
                    else return;
                }
            }

            listPlaylist.Items[nextIndex].Selected = true;
            listPlaylist.Select();
            PlaySelected();
        }

        private void PlayPrev()
        {
            if (listPlaylist.Items.Count == 0) return;

            int prevIndex = -1;

            if (_shuffle)
            {
                if (_shuffledOrder.Count == 0) RebuildShuffle();

                if (_currentIndex < 0)
                {
                    prevIndex = _shuffledOrder[0];
                }
                else
                {
                    int pos = _shuffledOrder.IndexOf(_currentIndex);
                    if (pos > 0) prevIndex = _shuffledOrder[pos - 1];
                    else
                    {
                        if (_repeat == RepeatMode.All) prevIndex = _shuffledOrder[_shuffledOrder.Count - 1];
                        else return;
                    }
                }
            }
            else
            {
                if (_currentIndex < 0) prevIndex = 0;
                else if (_currentIndex - 1 >= 0) prevIndex = _currentIndex - 1;
                else
                {
                    if (_repeat == RepeatMode.All) prevIndex = listPlaylist.Items.Count - 1;
                    else return;
                }
            }

            listPlaylist.Items[prevIndex].Selected = true;
            listPlaylist.Select();
            PlaySelected();
        }

        private void RebuildShuffle()
        {
            _shuffledOrder = Enumerable.Range(0, listPlaylist.Items.Count).ToList();

            if (_shuffle)
            {
                // Fisher–Yates
                for (int i = _shuffledOrder.Count - 1; i > 0; i--)
                {
                    int j = _rng.Next(i + 1);
                    (_shuffledOrder[i], _shuffledOrder[j]) = (_shuffledOrder[j], _shuffledOrder[i]);
                }
            }
        }

        private bool IsCurrentFile(string path)
        {
            try
            {
                var cur = _player.URL;
                return !string.IsNullOrEmpty(cur) &&
                       string.Equals(cur, path, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }
        #endregion

        #region Progress
        private void timerProgress_Tick(object sender, EventArgs e)
        {
            try
            {
                var media = _player.currentMedia;
                if (media == null || media.duration <= 0) return;

                double pos = _player.controls.currentPosition;
                double dur = media.duration;
                int value = (int)Math.Max(0, Math.Min(1000, (pos / dur) * 1000.0));
                trackBarProgress.Value = value;
            }
            catch
            {
                // ignore transient COM errors
            }
        }

        private void trackBarProgress_Scroll(object sender, EventArgs e)
        {
            try
            {
                var media = _player.currentMedia;
                if (media == null || media.duration <= 0) return;

                double dur = media.duration;
                double targetPos = (trackBarProgress.Value / 1000.0) * dur;
                _player.controls.currentPosition = targetPos;
            }
            catch { }
        }
        #endregion
    }
}
