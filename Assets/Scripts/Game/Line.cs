using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Scripts
{
    public class Line
    {
        private float startTime;
        private float endTime;
        private string text;

        public float StartTime { get => startTime; }
        public float EndTime { get => endTime; }
        public string Text { get => text; }

        public Line(Dictionary<string, string> lineDict)
        {
            this.startTime = float.Parse(lineDict["start"], System.Globalization.CultureInfo.InvariantCulture);
            this.endTime = float.Parse(lineDict["end"], System.Globalization.CultureInfo.InvariantCulture);
            this.text = lineDict["text"];
        }
    }
}
