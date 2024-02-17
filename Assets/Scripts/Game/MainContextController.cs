using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainContextController : MonoBehaviour
    {
        [SerializeField] private Text[] categoryTexts = new Text[7];
        [SerializeField] private Button[] categoryButtons = new Button[7];

        void Update()
        {
            if (GameManager.CurrentGameContext == GameContext.MainContext)
                CategoryButtonsInteractivityUpdate();
        }

        public void OnCategoryShowButtonClick(int categoryId)
        {
            GameManager.ChoosedCategories.Add(categoryId);
            GameManager.CurrentGameContext = GameContext.CategoryContext;
        }

        private void CategoryButtonsInteractivityUpdate()
        {
            foreach (var catBut in categoryButtons)
                catBut.interactable = !GameManager.OptionsShown;
            foreach (var cat in GameManager.ChoosedCategories)
                categoryButtons[cat].interactable = false;
        }
    }
}
