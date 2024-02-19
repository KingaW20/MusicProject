using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Category
    {
        private string name;
        private List<string> allSongTitles;
        private List<Song> selectedSongs;

        public string Name { get => name; }
        public List<string> AllSongTitles { get => allSongTitles; }
        public List<Song> SelectedSongs { get => selectedSongs; }

        public Category(string path)
        {
            this.name = Path.GetFileName(path);
            this.allSongTitles = new();
            this.selectedSongs = new();

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                var songFileNames = files.Where(file => file.EndsWith(".json", StringComparison.OrdinalIgnoreCase)).ToArray();
                var selectedSongFileNames = songFileNames.OrderBy(song => GameManager.Rand.Next()).Take(Constants.SONG_NUMBER).ToArray();

                foreach (var songFile in songFileNames)
                {
                    var songTitle = Path.GetFileNameWithoutExtension(songFile);
                    this.allSongTitles.Add(songTitle);
                }

                foreach (var songFile in selectedSongFileNames)
                {
                    var songTitle = Path.GetFileNameWithoutExtension(songFile);
                    string songJson = File.ReadAllText(Path.Combine(path, songFile));
                    this.selectedSongs.Add(new Song(songTitle, songJson));
                }
            }
            else
            {
                Debug.Log($"Path {path} doesn't exist");
            }
        }
    }
}

