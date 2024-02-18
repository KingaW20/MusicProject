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
        private List<Song> songs;

        public string Name { get => name; }
        public List<Song> Songs { get => songs; }

        public Category(string path)
        {
            this.name = Path.GetFileName(path);
            this.songs = new();

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                var songs = files.Where(file => file.EndsWith(".json", StringComparison.OrdinalIgnoreCase)).ToArray();

                foreach (var songFile in songs)
                {
                    var songTitle = Path.GetFileNameWithoutExtension(songFile);
                    string songJson = File.ReadAllText(Path.Combine(path, songFile));
                    this.songs.Add(new Song(songTitle, songJson));
                }
            }
            else
            {
                Debug.Log($"Path {path} doesn't exist");
            }
        }

        public List<string> GetSongsTitles()
        {
            List<string> songsTitles = new();
            foreach(var song in songs)
            {
                songsTitles.Add(song.Title);
            }
            return songsTitles;
        }
    }
}

