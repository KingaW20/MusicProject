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
        public static string CATEGORIES_PATH = Path.Combine(Application.dataPath, "Resources/Songs");
        public const int CATEGORY_NUMBER = 7;
    }

    public static class GameManager
    {
        public static SongManager SongManager;
        public static System.Random Rand;
        public static bool[] HelpUsed;
        public static List<int> ChoosedCategories;
        public static int CurrentSongId;
        public static GameContext CurrentGameContext;
        public static int AnswerWordNumber;
        public static int CorrectAnswers;
        public static bool LastAnswerCorrect;
        public static bool Answered;
        public static bool OptionsShown;

        //help data
        public static int ChoosedCategoryToChange;
        public static List<int> ChoosedHelpWords;

        public static void Setup()
        {
            HelpUsed = new bool[3] { false, false, false };
            ChoosedCategories = new();
            ChoosedHelpWords = new();
            AnswerWordNumber = 3;
            CorrectAnswers = 0;
            LastAnswerCorrect = true;
            Answered = false;
            OptionsShown = false;
            Rand = new();
            SongManager = new();
        }

        //TODO: class Category
        public static int GetCurrentCategory()
        {
            return ChoosedCategories.LastOrDefault();
        }

        //TODO: class Song
        public static int GetCurrentSong()
        {
            return CurrentSongId;
        }
    }
}
