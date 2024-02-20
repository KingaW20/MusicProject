using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public static class SongManager
    {
        public static int CurrentCategoryId;
        public static int CurrentSongId;
        public static int CurrentLineId;
        public static string SongSourcePath;
        public static float CurrentTime;
        public static bool IsAnswered;

        public static void Setup()
        {
            CurrentCategoryId = 0;
            CurrentSongId = 0;
            CurrentTime = 0f;
            CurrentLineId = 0;
            SongSourcePath = "";
            IsAnswered = false;
        }

        public static bool IsAnswerCorrect(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            answer = string.Join(" ", answerWords);
            return answer.ToLower() == GetCurrentSong().Answer.ToLower();
        }

        public static string GetCategoryNameById(int id)
        {
            if (GameManager.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return "";
            return GameManager.SelectedCategories[id].Name;
        }

        public static string GetCurrentCatSongTitleById(int id)
        {
            if (GameManager.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return "";
            return GameManager.SelectedCategories[CurrentCategoryId].SelectedSongs[id].Title;
        }

        public static Song GetCurrentSong()
        {
            if (GameManager.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return null;
            if (CurrentSongId < Constants.SONG_NUMBER)
                return GameManager.SelectedCategories[CurrentCategoryId].SelectedSongs[CurrentSongId];
            else
                return GameManager.HitSong;
        }
        
        public static string GetCurrentLine()
        {
            var line = GetCurrentSong().Lines[CurrentLineId];
            if (line.EndTime <= CurrentTime && CurrentLineId < GetCurrentSong().AnswerLineId - 1)
                CurrentLineId++;
            return GetCurrentSong().Lines[CurrentLineId].Text;
        }

        public static string GetWordFromCurrentSongAnswerById(int wordId)
        {
            string answer = GetCurrentSong().Answer;
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords[wordId].ToUpper();
        }

        public static bool IsSongEnded()
        {
            if (GetCurrentSong() == null)
                return false;
            return CurrentTime >= GetCurrentSong().StopTime;
        }
    }
}
