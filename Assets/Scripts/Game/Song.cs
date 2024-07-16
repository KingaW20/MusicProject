using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Song
    {
        private string categoryName;
        private string title;
        private List<Line> lines;
        private string answer;
        private int answerLineId;
        private string nextLine;
        private float stopTime;

        public string CategoryName { get => categoryName; }
        public string Title { get => title; }
        public List<Line> Lines { get => lines; }
        public string Answer { get => answer; }
        public int AnswerLineId { get => answerLineId; }
        public string NextLine { get => nextLine; }
        public float StopTime { get => stopTime; }

        public Song(string categoryName, string title, string songJsons)
        {
            this.categoryName = categoryName;
            this.title = title;
            this.lines = new();
            this.answer = "";

            try
            {
                var songsDicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(songJsons);
                foreach (var songDict in songsDicts)
                {
                    this.lines.Add(new Line(songDict));
                }
            }
            catch (JsonReaderException ex)
            {
                Debug.Log("Incorrect format of JSON song file: " + ex.Message);
            }
        }

        public string GetSongFilePathInResources()
        {
            return Path.Combine(categoryName, title);
        }

        public void RandomizeAnswer()
        {
            int max = this.lines.Count - Constants.BLOCK_LINES_NUMBER;
            int min = this.lines.Count > 2 * Constants.BLOCK_LINES_NUMBER ? 
                Constants.BLOCK_LINES_NUMBER : (this.lines.Count -  Constants.BLOCK_LINES_NUMBER) / 2;
            int answerLineId = GameManager.Rand.Next(min, max);
            this.answerLineId = answerLineId;
            this.stopTime = this.lines[answerLineId].StartTime;
            string[] answerWords;

            do
            {
                this.answer += this.lines[answerLineId].Text.Replace("\n", " ") + " ";
                answerWords = Line.GetTextWords(this.answer);
                answerLineId++;
            } while (answerWords.Length <= GameManager.AnswerWordNumber);

            this.answer = string.Join(" ", Line.GetTextWordsPart(answerWords, GameManager.AnswerWordNumber));
            Debug.Log("Czas: " + this.stopTime);
            Debug.Log("OdpowiedŸ: " + this.answer);

            string[] restLineAfterAnswerWords = Line.GetTextWordsPart(answerWords, 
                answerWords.Length - GameManager.AnswerWordNumber, GameManager.AnswerWordNumber);
            string[] lineAfterAnswerWords = Line.GetTextWords(this.lines[answerLineId].Text.Replace("\n", " "));
            string[] nextLineWords = restLineAfterAnswerWords.Concat(lineAfterAnswerWords).ToArray();
            this.nextLine = string.Join(" ", nextLineWords);
            Debug.Log("Kolejna linijka: " + this.nextLine);
        }
    }
}
