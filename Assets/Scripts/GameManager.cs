using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public enum GameContext
    {
        MainContext,
        CategoryContext,
        SongContext
    }

    public static class Constants
    {
        public static string CATEGORIES_PATH = Path.Combine(Application.dataPath, "Resources");
        public const int CATEGORY_NUMBER = 7;
        public const int SONG_NUMBER = 2;
        public const int HELP_NUMBER = 3;
        public const int HELP_WORDS = 2;
        public const int BLOCK_LINES_NUMBER = 5;
    }

    public static class GameManager
    {
        public static System.Random Rand;
        public static SongManager SongManager;
        public static List<int> ChoosedCategories;
        public static GameContext CurrentGameContext;
        public static bool OptionsShown;

        //answering
        public static int AnswerWordNumber;
        public static int CorrectAnswers;
        public static bool LastAnswerCorrect;

        //help data
        public static bool[] HelpUsed;
        public static List<int> ChoosedHelpWords;

        public static void Setup()
        {
            Rand = new();
            SongManager = new();
            ChoosedCategories = new();
            OptionsShown = false;

            AnswerWordNumber = 3;
            CorrectAnswers = 0;
            LastAnswerCorrect = true;

            HelpUsed = new bool[3] { false, false, false };
            ChoosedHelpWords = new();
        }

        public static bool IsAnswerTooLong(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords.Length > AnswerWordNumber;
        }
    }
}
