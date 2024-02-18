using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Scripts
{
    public class Line
    {
        private double startTime;
        private double endTime;
        private string text;

        public double StartTime { get => startTime; }
        public double EndTime { get => endTime; }
        public string Text { get => text; }

        public Line(Dictionary<string, string> lineDict)
        {
            this.startTime = double.Parse(lineDict["start"], System.Globalization.CultureInfo.InvariantCulture);
            this.endTime = double.Parse(lineDict["end"], System.Globalization.CultureInfo.InvariantCulture);
            this.text = lineDict["text"];
        }
    }
}
