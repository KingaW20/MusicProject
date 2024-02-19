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
        private string answer;
        private int answerLineId;
        private string nextLine;
        private float stopTime;

        public string Title { get => title; }
        public List<Line> Lines { get => lines; }
        public string Answer { get => answer; }
        public int AnswerLineId { get => answerLineId; }
        public string NextLine { get => nextLine; }
        public float StopTime { get => stopTime; }

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

        public string GetSongFilePath()
        {
            return Path.Combine(GameManager.SongManager.SelectedCategories[GameManager.SongManager.CurrentCategoryId].Name,
                GameManager.SongManager.GetCurrentSong().Title);
        }

        public void RandomizeAnswer()
        {
            int min = Constants.BLOCK_LINES_NUMBER;
            int max = lines.Count > 2* Constants.BLOCK_LINES_NUMBER ? lines.Count - Constants.BLOCK_LINES_NUMBER : lines.Count - 2;
            min = max <= min ? 2 : min;
            int answerLineId = GameManager.Rand.Next(min, max);
            this.answerLineId = answerLineId;
            this.stopTime = this.lines[answerLineId].StartTime;
            this.answer = "";
            string[] answerWords;

            do
            {
                this.answer += this.lines[answerLineId].Text + " ";
                answerWords = this.answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                answerLineId++;
            } while (answerWords.Length <= GameManager.AnswerWordNumber);

            string[] properAnswer = new string[GameManager.AnswerWordNumber];
            Array.Copy(answerWords, 0, properAnswer, 0, GameManager.AnswerWordNumber);
            this.answer = string.Join(" ", properAnswer);

            Debug.Log("Czas: " + this.stopTime);
            Debug.Log("OdpowiedŸ: " + this.answer);

            int restLen = answerWords.Length - GameManager.AnswerWordNumber;
            string[] nextWords = this.lines[answerLineId].Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int nextLen = nextWords.Length;

            string[] nextLineWords = new string[restLen + nextLen];
            Array.Copy(answerWords, GameManager.AnswerWordNumber, nextLineWords, 0, restLen);
            Array.Copy(nextWords, 0, nextLineWords, restLen, nextLen);
            this.nextLine = string.Join(" ", nextLineWords);
            Debug.Log("Kolejna: " + this.nextLine);
        }
    }
}
