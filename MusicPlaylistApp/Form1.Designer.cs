namespace MusicPlaylistApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView listPlaylist;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.CheckBox chkShuffle;
        private System.Windows.Forms.ComboBox cmbRepeat;
        private System.Windows.Forms.TrackBar trackBarProgress;
        private System.Windows.Forms.Label lblNowPlaying;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel pnlTopButtons;
        private System.Windows.Forms.Panel pnlBottom;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listPlaylist = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.chkShuffle = new System.Windows.Forms.CheckBox();
            this.cmbRepeat = new System.Windows.Forms.ComboBox();
            this.trackBarProgress = new System.Windows.Forms.TrackBar();
            this.lblNowPlaying = new System.Windows.Forms.Label();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pnlTopButtons = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProgress)).BeginInit();
            this.pnlTopButtons.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // listPlaylist
            // 
            this.listPlaylist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPlaylist.FullRowSelect = true;
            this.listPlaylist.HideSelection = false;
            this.listPlaylist.MultiSelect = false;
            this.listPlaylist.View = System.Windows.Forms.View.Details;
            this.listPlaylist.Location = new System.Drawing.Point(0, 48);
            this.listPlaylist.Name = "listPlaylist";
            this.listPlaylist.Size = new System.Drawing.Size(924, 421);
            this.listPlaylist.TabIndex = 0;
            this.listPlaylist.UseCompatibleStateImageBehavior = false;
            // 
            // pnlTopButtons
            // 
            this.pnlTopButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopButtons.Height = 48;
            this.pnlTopButtons.Padding = new System.Windows.Forms.Padding(8);
            this.pnlTopButtons.Controls.Add(this.btnAdd);
            this.pnlTopButtons.Controls.Add(this.btnRemove);
            this.pnlTopButtons.Controls.Add(this.btnPrev);
            this.pnlTopButtons.Controls.Add(this.btnPlay);
            this.pnlTopButtons.Controls.Add(this.btnPause);
            this.pnlTopButtons.Controls.Add(this.btnStop);
            this.pnlTopButtons.Controls.Add(this.btnNext);
            this.pnlTopButtons.Controls.Add(this.chkShuffle);
            this.pnlTopButtons.Controls.Add(this.cmbRepeat);
            // 
            // Buttons
            // 
            int btnW = 80, btnH = 30, gap = 8, x = 8, y = 9;

            this.btnAdd.Location = new System.Drawing.Point(x, y);
            this.btnAdd.Size = new System.Drawing.Size(btnW, btnH);
            this.btnAdd.Text = "Add";
            x += btnW + gap;

            this.btnRemove.Location = new System.Drawing.Point(x, y);
            this.btnRemove.Size = new System.Drawing.Size(btnW, btnH);
            this.btnRemove.Text = "Remove";
            x += btnW + gap;

            this.btnPrev.Location = new System.Drawing.Point(x, y);
            this.btnPrev.Size = new System.Drawing.Size(btnW, btnH);
            this.btnPrev.Text = "⟨ Prev";
            x += btnW + gap;

            this.btnPlay.Location = new System.Drawing.Point(x, y);
            this.btnPlay.Size = new System.Drawing.Size(btnW, btnH);
            this.btnPlay.Text = "Play";
            x += btnW + gap;

            this.btnPause.Location = new System.Drawing.Point(x, y);
            this.btnPause.Size = new System.Drawing.Size(btnW, btnH);
            this.btnPause.Text = "Pause";
            x += btnW + gap;

            this.btnStop.Location = new System.Drawing.Point(x, y);
            this.btnStop.Size = new System.Drawing.Size(btnW, btnH);
            this.btnStop.Text = "Stop";
            x += btnW + gap;

            this.btnNext.Location = new System.Drawing.Point(x, y);
            this.btnNext.Size = new System.Drawing.Size(btnW, btnH);
            this.btnNext.Text = "Next ⟩";
            x += btnW + gap;

            this.chkShuffle.Location = new System.Drawing.Point(x, y + 6);
            this.chkShuffle.AutoSize = true;
            this.chkShuffle.Text = "Shuffle";
            x += 80 + gap;

            this.cmbRepeat.Location = new System.Drawing.Point(x, y + 3);
            this.cmbRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRepeat.Size = new System.Drawing.Size(100, 23);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Height = 72;
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBottom.Controls.Add(this.trackBarProgress);
            this.pnlBottom.Controls.Add(this.lblNowPlaying);
            // 
            // trackBarProgress
            // 
            this.trackBarProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarProgress.Location = new System.Drawing.Point(8, 8);
            this.trackBarProgress.Minimum = 0;
            this.trackBarProgress.Maximum = 1000;
            this.trackBarProgress.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarProgress.Name = "trackBarProgress";
            this.trackBarProgress.Size = new System.Drawing.Size(908, 45);
            // 
            // lblNowPlaying
            // 
            this.lblNowPlaying.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNowPlaying.AutoSize = true;
            this.lblNowPlaying.Text = "—";
            this.lblNowPlaying.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 500;
            this.timerProgress.Enabled = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = "Audio|*.mp3;*.wav;*.wma;*.m4a|All files|*.*";
            this.openFileDialog1.Title = "Add audio files";
            // 
            // Form1
            // 
            this.Text = "Music Playlist App";
            this.ClientSize = new System.Drawing.Size(924, 541);
            this.Controls.Add(this.listPlaylist);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTopButtons);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.trackBarProgress)).EndInit();
            this.pnlTopButtons.ResumeLayout(false);
            this.pnlTopButtons.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
