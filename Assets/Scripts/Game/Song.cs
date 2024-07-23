using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class Song
    {
        [SerializeField] private string categoryName;
        [SerializeField] private string title;
        [NonSerialized] private List<Line> lines;
        [SerializeField] private string answer;
        [SerializeField] private int answerLineId;
        [SerializeField] private string nextLine;
        [SerializeField] private float stopTime;

        public string CategoryName { get => categoryName; }
        public string Title { get => title; }
        public List<Line> Lines { get => lines; }
        public string Answer { get => answer; }
        public int AnswerLineId { get => answerLineId; }
        public string NextLine { get => nextLine; }
        public float StopTime { get => stopTime; }

        public Song(string categoryName, string title)
        {
            this.categoryName = categoryName;
            this.title = title;
            this.lines = new();
            this.answer = "";
        }

        public Song ReadSong()
        {
            this.lines = new();
            var songFile = Resources.Load<TextAsset>(this.categoryName + "/" + this.Title);
            try
            {
                var songDicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(songFile.text);
                foreach (var songDict in songDicts)
                {
                    this.lines.Add(new Line(songDict));
                }
            }
            catch (JsonReaderException ex)
            {
                Debug.Log("Incorrect format of JSON song file: " + ex.Message);
            }

            return this;
        }

        public Song(Song s)
        {
            categoryName = s.CategoryName;
            title = s.Title;
            //lines = s.Lines.Select(item => new Line(item)).ToList();
            answer = s.Answer;
            answerLineId = s.AnswerLineId;
            nextLine = s.NextLine;
            stopTime = s.StopTime;
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
            } while (answerWords.Length <= GameManager.State.AnswerWordNumber);

            this.answer = string.Join(" ", Line.GetTextWordsPart(answerWords, GameManager.State.AnswerWordNumber));
            Debug.Log("Czas: " + this.stopTime);
            Debug.Log("OdpowiedŸ: " + this.answer);

            string[] restLineAfterAnswerWords = Line.GetTextWordsPart(answerWords, 
                answerWords.Length - GameManager.State.AnswerWordNumber, GameManager.State.AnswerWordNumber);
            string[] lineAfterAnswerWords = Line.GetTextWords(this.lines[answerLineId].Text.Replace("\n", " "));
            string[] nextLineWords = restLineAfterAnswerWords.Concat(lineAfterAnswerWords).ToArray();
            this.nextLine = string.Join(" ", nextLineWords);
            Debug.Log("Kolejna linijka: " + this.nextLine);
        }
    }
}
