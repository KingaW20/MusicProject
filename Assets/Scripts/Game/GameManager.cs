using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

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

    public static class Constants
    {
        public static string CATEGORIES_PATH = Path.Combine(Application.dataPath, "Resources");
        public const int CATEGORY_NUMBER = 7;
        public const int SONG_NUMBER = 2;
        public const int HELP_NUMBER = 3;
        public const int HELP_WORDS = 2;
        public const int BLOCK_LINES_NUMBER = 5;
        public const int FIRST_WORD_NUMBER = 3;
        public const int SAVING_SLOTS_NUMBER = 6;

        public static Color POSITIVE_COLOR = new Color(0f, 1f, 0f);
        public static Color NEGATIVE_COLOR = new Color(1f, 0f, 0f);
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
        public static List<string> AllCategoryNames;

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

        private static void ReadCategories()
        {
            var path = Constants.CATEGORIES_PATH;
            AllCategoryNames = new();

            string[] categoriesPath = Directory.GetDirectories(Constants.CATEGORIES_PATH);
            AllCategoryNames.AddRange(categoriesPath.Select(categoryPath => Path.GetFileName(categoryPath)).ToList());
        }

        public static void LoadChoosedCategories()
        {
            var path = Constants.CATEGORIES_PATH;
            State.SelectedCategories = new();

            string[] categoriesPath = Directory.GetDirectories(Constants.CATEGORIES_PATH);
            string[] selectedCategoriesPath = categoriesPath.Where(catPath => AllCategoryNames.Contains(Path.GetFileName(catPath)))
                .OrderBy(cat => Rand.Next()).Take(Constants.CATEGORY_NUMBER).ToArray();
            State.SelectedCategories.AddRange(selectedCategoriesPath.Select(categoryPath => new Category(categoryPath)).ToList());

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
            var restCategories = AllCategoryNames.Where(catName => !State.SelectedCategories.Any(cat => cat.Name == catName)).ToList();
            var catName = restCategories.OrderBy(cat => Rand.Next()).FirstOrDefault();
            return new Category(Path.Combine(Constants.CATEGORIES_PATH, catName));
        }

        private static Song ChooseHitSong()
        {
            var mainPath = Constants.CATEGORIES_PATH;

            List<string> songPaths = new();
            foreach (var categoryName in AllCategoryNames)
            {
                string path = Path.Combine(mainPath, categoryName);
                var songFileNames = Directory.GetFiles(path)
                    .Where(file => file.EndsWith(".json", StringComparison.OrdinalIgnoreCase)).ToArray();
                foreach (var songFileName in songFileNames)
                {
                    var songTitle = Path.GetFileNameWithoutExtension(songFileName);
                    if (State.SelectedCategories.Exists(cat => cat.Name == categoryName))
                        if (IsSongToChoose(songTitle, categoryName))
                            songPaths.Add(Path.Combine(path, songFileName));
                    else if (State.CategoryForChange.Name == categoryName)
                        if (IsSongToChooseFromCategoryToChoose(songTitle))
                            songPaths.Add(Path.Combine(path, songFileName));
                    else
                        songPaths.Add(Path.Combine(path, songFileName));
                }
            }

            var hitSongPath = songPaths.OrderBy(song => Rand.Next()).FirstOrDefault();
            var songJson = File.ReadAllText(hitSongPath);
            var hitCategoryName = new DirectoryInfo(Path.GetDirectoryName(hitSongPath)).Name;
            return new Song(hitCategoryName, Path.GetFileNameWithoutExtension(hitSongPath), songJson);
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
