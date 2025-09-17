# 🎵 Music Playlist App (WinForms + SQLite + Windows Media Player)

A lightweight Windows desktop music player built with **C# WinForms (.NET Framework 4.8)**.  
Features a persistent **SQLite** playlist, playback via **Windows Media Player (WMPLib)**, **Shuffle**, **Repeat (None / One / All)**, and a **seek bar**.

---

## ✨ Features

- Add multiple audio files at once (`.mp3`, `.wav`, `.wma`, `.m4a`)
- Persistent playlist stored in **SQLite** (`playlist.db`)
- Playback controls: **Play / Pause / Stop / Prev / Next**
- **Shuffle** toggle (Fisher–Yates random order)
- **Repeat** modes: **None**, **One**, **All**
- **Seek bar** to scrub within the current track
- “Now Playing” indicator
- Double-click any row to play

> ⚠️ Windows-only (uses Windows Media Player COM API).

---

## 🧰 Tech Stack

- **C# WinForms** — .NET Framework **4.8**
- **SQLite** — via `System.Data.SQLite.Core` NuGet
- **Windows Media Player COM** — `WMPLib` (headless, no AxHost UI)

---

## 🚀 Getting Started

### 1) Requirements
- Windows 10/11
- **Visual Studio 2022** (Community OK)
- **.NET Framework 4.8** developer pack
- **Windows Media Player** enabled  
  (`Control Panel → Turn Windows features on or off → Media Features → Windows Media Player`)

### 🗂️ Project Structure

MusicPlaylistApp/

├─ Program.cs

├─ Form1.cs

├─ Form1.Designer.cs
├─ App.config                (optional)

├─ packages.config           (auto from NuGet)

└─ playlist.db               (created at runtime)

---

## ⌨️ Basic Usage

- Double-click a song → play it

- Shuffle checkbox → random order on Next/Prev

- Repeat dropdown → None, One, All

- Prev / Next → navigate by current order (shuffled or linear)

- Seek bar → drag to jump within the track
---

## Screen Shots

### 🎵 Music Playlist App
<img src="./music .png" alt="Main Window" width="500"/>
