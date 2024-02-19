using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class SongManager
    {
        private List<string> allCategoriesNames;
        private List<Category> selectedCategories;
        private int currentCategoryId;
        private int currentSongId;
        private int currentLineId;
        private string songSourcePath;
        private float currentTime;
        private bool isAnswered;
        private Category categoryForChange;
        private Song hitSong;
        private string hitSongFilePath;

        public List<string> AllCategoriesNames { get => allCategoriesNames; }
        public List<Category> SelectedCategories { get => selectedCategories; }
        public int CurrentCategoryId { get => currentCategoryId; set => currentCategoryId = value; }
        public int CurrentSongId { get => currentSongId; set => currentSongId = value; }
        public int CurrentLineId { get => currentLineId; set => currentLineId = value; }
        public string SongSourcePath { get => songSourcePath; set => songSourcePath = value; }
        public float CurrentTime { get => currentTime; set => currentTime = value; }
        public bool IsAnswered { get => isAnswered; set => isAnswered = value; }
        public Category CategoryForChange { get => categoryForChange; }
        public Song HitSong { get => hitSong; }
        public string HitSongFilePath { get => hitSongFilePath; }

        public SongManager()
        {
            this.allCategoriesNames = new();
            this.selectedCategories = new();
            this.currentTime = 0f;
            this.currentLineId = 0;
            this.songSourcePath = "";
            this.isAnswered = false;
            var path = Constants.CATEGORIES_PATH;

            if (Directory.Exists(path))
            {
                string[] categoriesPath = Directory.GetDirectories(Constants.CATEGORIES_PATH);
                string[] selectedCategoriesPath = categoriesPath.OrderBy(cat => GameManager.Rand.Next()).Take(Constants.CATEGORY_NUMBER).ToArray();

                foreach (var categoryPath in categoriesPath)
                {
                    allCategoriesNames.Add(Path.GetFileName(categoryPath));
                }

                foreach (var categoryPath in selectedCategoriesPath)
                {
                    selectedCategories.Add(new Category(categoryPath));
                }
            }
            else
            {
                Debug.Log($"Path {path} doesn't exist");
            }

            this.categoryForChange = GetRandomCategoryFromRest();
            this.hitSong = ChooseHitSong();
        }

        public bool IsAnswerCorrect(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            answer = string.Join(" ", answerWords).ToLower();
            return answer == GetCurrentSongAnswer();
        }

        public string GetCategoryNameById(int id)
        {
            return selectedCategories[id].Name.ToUpper();
        }

        public string GetCurrentCatSongTitleById(int id)
        {
            return selectedCategories[this.currentCategoryId].SelectedSongs[id].Title.ToUpper();
        }

        public Song GetCurrentSong()
        {
            if (this.currentSongId < Constants.SONG_NUMBER)
                return selectedCategories[this.currentCategoryId].SelectedSongs[this.currentSongId];
            else
                return this.hitSong;
        }

        public string GetCurrentSongAnswer()
        {
            return GetCurrentSong().Answer.ToLower();
        }
        
        public string GetCurrentLine()
        {
            var line = GetCurrentSong().Lines[this.currentLineId];
            if (line.EndTime <= this.currentTime)
                this.currentLineId++;
            int answerLineId = GetCurrentSong().AnswerLineId;
            this.currentLineId = this.currentLineId >= answerLineId ? answerLineId - 1 : this.currentLineId;
            return GetCurrentSong().Lines[this.currentLineId].Text;
        }

        public string GetWordIdFromCurrentSongAnswer(int wordId)
        {
            string answer = GetCurrentSongAnswer();
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords[wordId].ToUpper();
        }

        public bool IsSongEnded()
        {
            return GameManager.SongManager.CurrentTime >= GameManager.SongManager.GetCurrentSong().StopTime;
        }

        public void ChangeCategory(int id)
        {
            this.selectedCategories[id] = this.categoryForChange;
        }

        public bool IsHit()
        {
            return GameManager.CorrectAnswers == Constants.CATEGORY_NUMBER;
        }

        private Category GetRandomCategoryFromRest()
        {
            var restCategories = this.allCategoriesNames.Where(catName => !selectedCategories.Any(cat => cat.Name == catName)).ToList();
            var catName = restCategories.OrderBy(cat => GameManager.Rand.Next()).FirstOrDefault();
            return new Category(Path.Combine(Constants.CATEGORIES_PATH, catName));
        }

        private Song ChooseHitSong()
        {
            var mainPath = Constants.CATEGORIES_PATH;

            List<string> songPaths = new();
            foreach(var categoryName in this.allCategoriesNames)
            {
                string path = Path.Combine(mainPath, categoryName);
                string[] files = Directory.GetFiles(path);
                var songFileNames = files.Where(file => file.EndsWith(".json", StringComparison.OrdinalIgnoreCase)).ToArray();
                foreach (var songFileName in songFileNames)
                {
                    var songTitle = Path.GetFileNameWithoutExtension(songFileName);
                    if (this.selectedCategories.Exists(cat => cat.Name == categoryName))
                    {
                        if (!selectedCategories.Where(cat => cat.Name == categoryName).ToList().FirstOrDefault()
                            .SelectedSongs.Exists(song => song.Title == songTitle))
                            songPaths.Add(Path.Combine(path, songFileName));
                    }
                    else if (this.categoryForChange.Name == categoryName)
                    {
                        if (!this.categoryForChange.SelectedSongs.Exists(song => song.Title == songTitle))
                            songPaths.Add(Path.Combine(path, songFileName));
                    }
                    else
                        songPaths.Add(Path.Combine(path, songFileName));
                }
            }

            string songPath = songPaths.OrderBy(song => GameManager.Rand.Next()).FirstOrDefault();
            string songJson = File.ReadAllText(songPath);
            var hitSongTitle = Path.GetFileNameWithoutExtension(songPath);
            var hitCategoryPath = Path.GetDirectoryName(songPath);
            var hitCategoryName = new DirectoryInfo(hitCategoryPath).Name;
            this.hitSongFilePath = Path.Combine(hitCategoryName, hitSongTitle);
            return new Song(Path.GetFileNameWithoutExtension(songPath), songJson);
        }
    }
}
