using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public Category(string name)
        {
            this.name = name;
            this.allSongTitles = new();
            this.selectedSongs = new();

            var songsJson = Resources.Load<TextAsset>(name + "/" + Constants.SONGS_FILENAME);
            if (songsJson != null)
            {
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(songsJson.text);
                this.allSongTitles = jsonData[Constants.SONGS_FILENAME];
                var selectedSongTitles = this.allSongTitles.OrderBy(songTitle => GameManager.Rand.Next()).Take(Constants.SONG_NUMBER).ToList();

                foreach (var songTitle in selectedSongTitles)
                    this.selectedSongs.Add(new Song(this.name, songTitle));
            }
            else
            {
                Debug.Log($"Nie ma pliku {name}/{Constants.SONGS_FILENAME}");
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

