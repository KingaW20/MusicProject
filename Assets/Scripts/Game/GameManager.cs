using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Scripts
{
    [Serializable]
    public enum MenuContext
    {
        MainContext,
        PlayContext,
        LoadContext
    }

    [Serializable]
    public enum GameContext
    {
        MainContext,
        CategoryContext,
        SongContext,
        EndContext
    }

    [Serializable]
    public enum CorrectnessType
    {
        Right,
        AlmostRight,
        Wrong
    }

    public static class Constants
    {
        public static string CATEGORIES_FILENAME = "categories";
        public static string SONGS_FILENAME = "songs";

        public const int CATEGORY_NUMBER = 7;
        public const int SONG_NUMBER = 2;
        public const int HELP_NUMBER = 3;
        public const int HELP_WORDS = 2;
        public const int BLOCK_LINES_NUMBER = 5;
        public const int FIRST_WORD_NUMBER = 3;
        public const int SAVING_SLOTS_NUMBER = 6;

        public static Color POSITIVE_COLOR = new Color(0f, 1f, 0f);
        public static Color NEGATIVE_COLOR = new Color(1f, 0f, 0f);
        public static Color ALMOST_RIGHT_ANSWER_COLOR = new Color(0f, 0f, 1f);
        public static Color NEUTRAL_COLOR = new Color(1f, 1f, 1f);
        public static Color WHITE = new Color(1f, 1f, 1f, 1f);
        public static Color WHITE_LITTLE_TRANSPARENT = new Color(1f, 1f, 1f, 0.85f);
        public static Color WHITE_TRANSPARENT = new Color(1f, 1f, 1f, 0.5f);
    }

    public static class GameManager
    {
        public static System.Random Rand;
        public static GameState State;
        public static bool OptionsShown;
        public static bool SaveWindowShown;
        public static bool JustChangedToSongContext;
        public static bool GameLoadedOnSong;

        //songs
        public static List<string> CategoryNames;

        //help data
        public static int ChoosedHelpWordsNumber;
        public static bool[] HelpShown;

        public static void Setup()
        {
            Rand = new();
            JustChangedToSongContext = false;
            OptionsShown = false;
            SaveWindowShown = false;
            GameLoadedOnSong = false;

            ReadCategories();
            State = new GameState();

            ChoosedHelpWordsNumber = 0;
            HelpShown = new bool[Constants.HELP_NUMBER];
            for (int i = 0; i < Constants.HELP_NUMBER; i++)
                HelpShown[i] = false;
        }

        /// <summary>
        /// Read all categories from file "categories" in folder "Resources"
        /// </summary>
        private static void ReadCategories()
        {
            CategoryNames = new();
            TextAsset categoriesJson = Resources.Load<TextAsset>(Constants.CATEGORIES_FILENAME);
            if (categoriesJson != null)
            {
                Dictionary<string, List<string>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(categoriesJson.text);
                CategoryNames = jsonData[Constants.CATEGORIES_FILENAME];
            }
        }

        public static void LoadChoosedCategories()
        {
            State.SelectedCategories = new();
            string[] selectedCategoryNames = CategoryNames.OrderBy(cat => Rand.Next()).Take(Constants.CATEGORY_NUMBER).ToArray();
            State.SelectedCategories.AddRange(selectedCategoryNames.Select(categoryName => new Category(categoryName)).ToList());

            State.CategoryForChange = GetRandomCategoryFromRest();
            State.HitSong = ChooseHitSong();
            SongManager.Setup();
        }

        public static bool IsAnswerTooLong(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords.Length > State.AnswerWordNumber;
        }

        public static bool IsHitTime()
        {
            return State.AnswersCorrectness.Count == Constants.CATEGORY_NUMBER;
        }

        private static Category GetRandomCategoryFromRest()
        {
            var restCategories = CategoryNames.Where(catName => !State.SelectedCategories.Any(cat => cat.Name == catName)).ToList();
            var catName = restCategories.OrderBy(cat => Rand.Next()).FirstOrDefault();
            return new Category(catName);
        }

        private static Song ChooseHitSong()
        {
            List<(string category, string title)> restSongs = new();
            foreach (var categoryName in CategoryNames)
            {
                var songsJson = Resources.Load<TextAsset>(categoryName + "/" + Constants.SONGS_FILENAME);
                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(songsJson.text);
                var songTitles = jsonData[Constants.SONGS_FILENAME];

                foreach (var songTitle in songTitles)
                {
                    if (State.SelectedCategories.Exists(cat => cat.Name == categoryName))
                    {
                        if (IsSongToChoose(songTitle, categoryName))
                            restSongs.Add((categoryName, songTitle));
                    }
                    else if (State.CategoryForChange.Name == categoryName)
                    {
                        if (IsSongToChooseFromCategoryToChoose(songTitle))
                            restSongs.Add((categoryName, songTitle));
                    }
                    else
                        restSongs.Add((categoryName, songTitle));
                }
            }

            var hitSong = restSongs.OrderBy(song => Rand.Next()).FirstOrDefault();
            var hitSongFile = Resources.Load<TextAsset>(hitSong.category + "/" + hitSong.title);
            return new Song(hitSong.category, hitSong.title);
        }

        private static bool IsSongToChoose(string songTitle, string categoryName)
        {
            return !State.SelectedCategories.Where(cat => cat.Name == categoryName).ToList().FirstOrDefault()
                .SelectedSongs.Exists(song => song.Title == songTitle);
        }

        private static bool IsSongToChooseFromCategoryToChoose(string songTitle)
        {
            return !State.CategoryForChange.SelectedSongs.Exists(song => song.Title == songTitle);
        }

        public static bool ShowOnHover(bool additionalCondition = true, Help helpType = Help.None)
        {
            bool help = helpType == Help.None || (!HelpShown[(int)helpType] && !State.HelpUsed[(int)helpType]);
            return help && !OptionsShown && !SaveWindowShown && additionalCondition;
        }
    }
}
