using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Scripts
{
    public class Song
    {
        private string title;
        private List<Line> lines;

        public string Title { get => title; }
        public List<Line> Lines { get => lines; }

        public Song(string title, string songJsons)
        {
            this.title = title;
            this.lines = new();

            try
            {
                var songsDicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(songJsons);
                foreach (var songDict in songsDicts)
                {
                    this.lines.Add(new Line(songDict));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Incorrect format of JSON song file: " + ex.Message);
            }
        }
    }
}
