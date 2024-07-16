using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class SongTest : MonoBehaviour
    {
        [SerializeField] public string categoryName;
        [SerializeField] public string songTitle;
        [SerializeField] public Text songLine;
        [SerializeField] public AudioSource songSource;
        [SerializeField] public float startTime = 0;

        private List<Line> lines;
        private int lineId;
        private float songTime;
        private Line line;

        void Start()
        {
            OnLoadSongButtonClick();
            //CheckAnswer();
        }

        void Update()
        {
            //song line update
            songTime = songSource.time;
            line = lines[lineId];
            if (songSource.isPlaying)
            {
                Debug.Log(songTime);
                if (line.StartTime <= songTime && songTime < line.EndTime)
                    songLine.text = lines[lineId].Text;
                else if (line.EndTime <= songTime && lineId < lines.Count - 1 && lines[lineId + 1].StartTime <= songTime)
                {
                    lineId++;
                    songLine.text = lines[lineId].Text;
                }
                else if (line.EndTime <= songTime)
                    songLine.text = "...";
            }
        }

        public void OnPlayButtonClick()
        {
            if (!songSource.isPlaying)
                songSource.Play();
            else 
                songSource.Pause();
        }

        public void OnLoadSongButtonClick()
        {
            ClearLog();
            
            //load song lines from json line
            string path = Path.Combine(Application.dataPath, "Resources", categoryName, songTitle + ".json");
            string songJson = File.ReadAllText(path);
            var songsDicts = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(songJson);
            lines = new();
            foreach (var songDict in songsDicts)
                lines.Add(new Line(songDict));

            //initial line 
            lineId = 0;
            songLine.text = "...";

            // audio
            songSource.clip = Resources.Load<AudioClip>(Path.Combine(categoryName, songTitle));
            songSource.time = startTime;
        }

        public void CheckAnswer()
        {
            int answerLineId = 16;
            string[] answerWords;
            string answer = "";
            int answerWordNumber = 11;

            do
            {
                answer += lines[answerLineId].Text.Replace("\n", " ") + " ";
                answerWords = Line.GetTextWords(answer);
                answerLineId++;
            } while (answerWords.Length <= answerWordNumber);

            answer = string.Join(" ", Line.GetTextWordsPart(answerWords, answerWordNumber));
            Debug.Log("OdpowiedŸ: " + answer);

            string[] restLineAfterAnswerWords = Line.GetTextWordsPart(answerWords,
                answerWords.Length - answerWordNumber, answerWordNumber);
            string[] lineAfterAnswerWords = Line.GetTextWords(lines[answerLineId].Text.Replace("\n", " "));
            string[] nextLineWords = restLineAfterAnswerWords.Concat(lineAfterAnswerWords).ToArray();
            string nextLine = string.Join(" ", nextLineWords);
            Debug.Log("Kolejna linijka: " + nextLine);
        }

        public void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}