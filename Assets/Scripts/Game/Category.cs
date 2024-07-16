using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class Category
    {
        [SerializeField] private string name;
        [SerializeField] private List<string> allSongTitles;
        [SerializeField] private List<Song> selectedSongs;

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
                    if (selectedSongFileNames.Contains(songFile))
                    {
                        string songJson = File.ReadAllText(Path.Combine(path, songFile));
                        this.selectedSongs.Add(new Song(this.name, songTitle, songJson));
                    }
                }
            }
            else
            {
                Debug.Log($"Path {path} doesn't exist");
            }
        }

        public Category(Category c)
        {
            name = c.Name;
            allSongTitles = c.AllSongTitles.ToList();
            selectedSongs = c.SelectedSongs.Select(item => new Song(item)).ToList();
        }
    }
}

