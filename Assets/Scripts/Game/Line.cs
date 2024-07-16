using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class Line
    {
        [SerializeField] private float startTime;
        [SerializeField] private float endTime;
        [SerializeField] private string text;

        public float StartTime { get => startTime; }
        public float EndTime { get => endTime; }
        public string Text { get => text; }

        public Line(Dictionary<string, string> lineDict)
        {
            this.startTime = float.Parse(lineDict["start"], System.Globalization.CultureInfo.InvariantCulture);
            this.endTime = float.Parse(lineDict["end"], System.Globalization.CultureInfo.InvariantCulture);
            this.text = lineDict["text"];
        }

        public Line(Line l)
        {
            startTime = l.StartTime;
            endTime = l.EndTime;
            text = l.Text;
        }

        public static string[] GetTextWords(string text)
        {
            return text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetTextWordsPart(string[] textWords, int partLength, int textStart = 0)
        {
            string[] textWordsPart = new string[partLength];
            Array.Copy(textWords, textStart, textWordsPart, 0, partLength);
            return textWordsPart;
        }
    }
}
