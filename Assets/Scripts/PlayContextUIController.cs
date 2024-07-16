using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Scripts
{
    public class PlayContextUIController : MonoBehaviour
    {
        [SerializeField] private Text[] categoryNames = new Text[12];
        [SerializeField] private Toggle[] toggles = new Toggle[12];
        [SerializeField] private Button previousListButton;
        [SerializeField] private Button nextListButton;
        [SerializeField] private Button playButton;

        private int page;
        private int pageSize;
        private List<string> currentCategoriesShown;
        private Dictionary<string, bool> categoriesChoosed;

        void Start()
        {
            this.page = 0;
            this.pageSize = 12;
            this.categoriesChoosed = GameManager.AllCategoryNames.ToDictionary(cat => cat, val => true);
            UpdateContent();
        }

        public void UpdateContent()
        {
            this.currentCategoriesShown = GameManager.AllCategoryNames.Skip(this.pageSize * this.page).Take(this.pageSize).ToList();

            for (int i = 0; i < this.currentCategoriesShown.Count; i++)
                categoryNames[i].text = this.currentCategoriesShown[i][0].ToString().ToUpper() + this.currentCategoriesShown[i].Substring(1);

            for (int i = 0; i < this.pageSize; i++)
            {
                toggles[i].gameObject.SetActive(i < this.currentCategoriesShown.Count);
                if (i < this.currentCategoriesShown.Count)
                    toggles[i].isOn = this.categoriesChoosed.Skip(this.pageSize * this.page).Take(this.pageSize).ElementAtOrDefault(i).Value;
            }

            previousListButton.interactable = this.page != 0;
            nextListButton.interactable = this.pageSize * (this.page + 1) < this.categoriesChoosed.Count;
        }

        public void OnCheckAllButtonClick()
        {
            this.categoriesChoosed = GameManager.AllCategoryNames.ToDictionary(cat => cat, val => true);
            for (int i = 0; i < this.pageSize; i++)
                toggles[i].isOn = this.categoriesChoosed.ElementAtOrDefault(i).Value;
        }

        public void OnUnCheckAllButtonClick()
        {
            this.categoriesChoosed = GameManager.AllCategoryNames.ToDictionary(cat => cat, val => false);
            for (int i = 0; i < this.pageSize; i++)
                toggles[i].isOn = this.categoriesChoosed.ElementAtOrDefault(i).Value;
        }

        public void OnPreviousListButtonClick()
        {
            this.page--;
            UpdateContent();
        }

        public void OnNextListButtonClick()
        {
            this.page++;
            UpdateContent();
        }

        public void OnToggleClick(int toggleId)
        {
            var categoryName = this.categoryNames[toggleId].text;
            this.categoriesChoosed[categoryName] = toggles[toggleId].isOn;
            playButton.interactable = this.categoriesChoosed.Where(pair => pair.Value == true)
                .ToDictionary(pair => pair.Key, pair => pair.Value).Count >= Constants.CATEGORY_NUMBER + 1;
        }

        public void OnPlayButtonClick()
        {
            GameManager.AllCategoryNames = this.categoriesChoosed.Where(pair => pair.Value == true).Select(pair => pair.Key).ToList();
            GameManager.LoadChoosedCategories();
            SceneManager.LoadScene("Game");
        }

        public void OnBackButtonClick()
        {
            GameManager.CurrentMenuContext = MenuContext.MainContext;
        }
    }
}
