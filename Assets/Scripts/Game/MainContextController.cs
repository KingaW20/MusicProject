using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainContextController : MonoBehaviour
    {
        [SerializeField] private Text[] categoryTexts = new Text[Constants.CATEGORY_NUMBER];
        [SerializeField] private Button[] categoryButtons = new Button[Constants.CATEGORY_NUMBER];

        void Update()
        {
            if (GameManager.CurrentGameContext == GameContext.MainContext)
            {
                if (GameManager.CurrentGameContext == GameContext.MainContext)
                    CategoryButtonsInteractivityUpdate();
                for (int i = 0; i < Constants.CATEGORY_NUMBER; i++)
                    categoryTexts[i].text = GameManager.SongManager.GetCategoryNameById(i);
            }
        }

        public void OnCategoryShowButtonClick(int categoryId)
        {
            GameManager.ChoosedCategories.Add(categoryId);
            GameManager.SongManager.CurrentCategoryId = categoryId;
            GameManager.CurrentGameContext = GameContext.CategoryContext;
        }

        private void CategoryButtonsInteractivityUpdate()
        {
            for (int id = 0; id < Constants.CATEGORY_NUMBER; id++)
                categoryButtons[id].interactable = !GameManager.ChoosedCategories.Contains(id) && !GameManager.OptionsShown;
        }
    }
}
