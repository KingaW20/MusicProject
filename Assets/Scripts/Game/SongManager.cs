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
        private string correctAnswer;

        public List<string> AllCategoriesNames { get => allCategoriesNames; }
        public List<Category> SelectedCategories { get => selectedCategories; }
        public string CorrectAnswer { get => correctAnswer; set => correctAnswer = value; }

        public SongManager()
        {
            this.allCategoriesNames = new();
            this.selectedCategories = new();
            this.correctAnswer = "";
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
        }

        public bool IsAnswerCorrect(string answer)
        {
            return answer.ToLower() == this.correctAnswer.ToLower();
        }

        public string GetCategoryNameById(int id)
        {
            return selectedCategories[id].Name.ToUpper();
        }

        public void ChangeCategory(int id)
        {
            this.selectedCategories[id] = GetRandomCategoryFromRest();
        }

        private Category GetRandomCategoryFromRest()
        {
            var restCategories = this.allCategoriesNames.Where(catName => !selectedCategories.Any(cat => cat.Name == catName)).ToList();
            var catName = restCategories.OrderBy(cat => GameManager.Rand.Next()).FirstOrDefault();
            return new Category(Path.Combine(Constants.CATEGORIES_PATH, catName));
        }
    }
}
