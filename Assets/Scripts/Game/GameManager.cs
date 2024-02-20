using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public enum MenuContext
    {
        MainContext,
        PlayContext,
        LoadContext
    }

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
        public const int FIRST_WORD_NUMBER = 3;

        public static Color POSITIVE_COLOR = new Color(0f, 1f, 0f);
        public static Color NEGATIVE_COLOR = new Color(1f, 0f, 0f);
        public static Color NEUTRAL_COLOR = new Color(1f, 1f, 1f);
    }

    public static class GameManager
    {
        public static System.Random Rand;
        public static List<int> ChoosedCategoryIds;
        public static MenuContext CurrentMenuContext;
        public static GameContext CurrentGameContext;
        public static bool OptionsShown;

        //songs
        public static List<string> AllCategoryNames;
        public static List<Category> SelectedCategories;
        public static Category CategoryForChange;
        public static Song HitSong;

        //answering
        public static int AnswerWordNumber;
        public static List<bool> AnswersCorrectness;

        //help data
        public static bool[] HelpUsed;
        public static int ChoosedHelpWordsNumber;
        public static bool[] HelpShown;

        public static void Setup()
        {
            Rand = new();
            ChoosedCategoryIds = new();
            CurrentMenuContext = MenuContext.MainContext;
            CurrentGameContext = GameContext.MainContext;
            OptionsShown = false;

            ReadCategories();

            AnswerWordNumber = Constants.FIRST_WORD_NUMBER;
            AnswersCorrectness = new();

            HelpUsed = new bool[3] { false, false, false };
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
            SelectedCategories = new();

            string[] categoriesPath = Directory.GetDirectories(Constants.CATEGORIES_PATH);
            string[] selectedCategoriesPath = categoriesPath.Where(catPath => AllCategoryNames.Contains(Path.GetFileName(catPath)))
                .OrderBy(cat => GameManager.Rand.Next()).Take(Constants.CATEGORY_NUMBER).ToArray();
            SelectedCategories.AddRange(selectedCategoriesPath.Select(categoryPath => new Category(categoryPath)).ToList());

            CategoryForChange = GetRandomCategoryFromRest();
            HitSong = ChooseHitSong();
            SongManager.Setup();
        }

        public static bool IsAnswerTooLong(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords.Length > AnswerWordNumber;
        }

        public static bool IsHitTime()
        {
            return AnswersCorrectness.Count == Constants.CATEGORY_NUMBER;
        }

        private static Category GetRandomCategoryFromRest()
        {
            var restCategories = AllCategoryNames.Where(catName => !SelectedCategories.Any(cat => cat.Name == catName)).ToList();
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
                    if (SelectedCategories.Exists(cat => cat.Name == categoryName))
                        if (IsSongToChoose(songTitle, categoryName))
                            songPaths.Add(Path.Combine(path, songFileName));
                    else if (CategoryForChange.Name == categoryName)
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
            return !SelectedCategories.Where(cat => cat.Name == categoryName).ToList().FirstOrDefault()
                .SelectedSongs.Exists(song => song.Title == songTitle);
        }

        private static bool IsSongToChooseFromCategoryToChoose(string songTitle)
        {
            return !CategoryForChange.SelectedSongs.Exists(song => song.Title == songTitle);
        }
    }
}
