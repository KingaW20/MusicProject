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
            if (GameManager.State.CurrentGameContext == GameContext.MainContext)
            {
                for (int id = 0; id < Constants.CATEGORY_NUMBER; id++)
                    categoryButtons[id].interactable = !GameManager.State.ChoosedCategoryIds.Contains(id) && !GameManager.OptionsShown;
                for (int i = 0; i < Constants.CATEGORY_NUMBER; i++)
                    categoryTexts[i].text = SongManager.GetCategoryNameById(i).ToUpper();
            }
        }

        public void OnCategoryShowButtonClick(int categoryId)
        {
            GameManager.State.ChoosedCategoryIds.Add(categoryId);
            SongManager.State.CurrentCategoryId = categoryId;
            GameManager.State.CurrentGameContext = GameContext.CategoryContext;
        }
    }
}
