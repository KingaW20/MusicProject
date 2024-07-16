using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public enum Help
    {
        TwoWords,
        Change,
        NextLine,
        None
    }

    public class HelpUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] helpBoxes = new GameObject[Constants.HELP_NUMBER];
        [SerializeField] private Button[] helpButtons = new Button[Constants.HELP_NUMBER];
        [SerializeField] private GameObject[] helpUsedImages = new GameObject[Constants.HELP_NUMBER];

        [SerializeField] private Button[] helpWordButtons = new Button[9];
        [SerializeField] private Button[] categoryToChangeButtons = new Button[Constants.CATEGORY_NUMBER];
        [SerializeField] private Text nextLineText;

        void Update()
        {
            HelpButtonTextsUpdate();
            HelpButtonsInteractivityUpdate();
            for (int i = 0; i < Constants.HELP_NUMBER; i++)
                helpBoxes[i].SetActive(GameManager.HelpShown[i]);
        }

        public void OnTwoWordsHelpButtonClick()
        {
            for (int i = 0; i < helpWordButtons.Length; i++)
                helpWordButtons[i].gameObject.SetActive(i < GameManager.State.AnswerWordNumber);
            ShowHelp(Help.TwoWords);
        }

        public void OnChooseWordButtonClick(int wordId)
        {
            GameManager.ChoosedHelpWordsNumber++;
            helpWordButtons[wordId].interactable = false;
            helpWordButtons[wordId].gameObject.GetComponent<Image>().color = Constants.POSITIVE_COLOR;
            helpWordButtons[wordId].GetComponentInChildren<Text>().text = SongManager.GetWordFromCurrentSongAnswerById(wordId);

            if (GameManager.ChoosedHelpWordsNumber == Constants.HELP_WORDS)
            {
                foreach (var helpWordButton in helpWordButtons)
                    helpWordButton.interactable = false;
                GameManager.State.HelpUsed[(int)Help.TwoWords] = true;
            }
        }

        public void OnChangeHelpButtonClick()
        {
            for (int id = 0; id < Constants.CATEGORY_NUMBER; id++)
                categoryToChangeButtons[id].interactable = !GameManager.State.ChoosedCategoryIds.Contains(id);
            ShowHelp(Help.Change);
        }

        public void OnChooseCategoryToChangeButtonClick(int categoryId)
        {
            foreach (var cat in categoryToChangeButtons)
                cat.interactable = false;
            categoryToChangeButtons[categoryId].gameObject.GetComponent<Image>().color = Constants.POSITIVE_COLOR;
            GameManager.State.HelpUsed[(int)Help.Change] = true;
            GameManager.State.SelectedCategories[categoryId] = GameManager.State.CategoryForChange;
        }

        public void OnNextLineHelpButtonClick()
        {
            nextLineText.text = SongManager.GetCurrentSong().NextLine;
            ShowHelp(Help.NextLine);
            GameManager.State.HelpUsed[(int)Help.NextLine] = true;
        }

        private void ShowHelp(Help help)
        {
            GameManager.HelpShown[(int)help] = !GameManager.HelpShown[(int)help];
            helpBoxes[(int)help].SetActive(GameManager.HelpShown[(int)help]);
        }

        private void HelpButtonsInteractivityUpdate()
        {
            for (int i = 0; i < Constants.HELP_NUMBER; i++)
                helpUsedImages[i].SetActive(GameManager.State.HelpUsed[i] && !helpBoxes[i].activeSelf);

            // if any of helps is shown set only button of this help interactable
            if (GameManager.HelpShown.Any(item => item == true))
            {
                for (int i = 0; i < Constants.HELP_NUMBER; i++)
                    helpButtons[i].interactable = GameManager.HelpShown[i] && GameManager.State.HelpUsed[i] && CanInteractWithHelp();
                return;
            }

            // make buttons interactable based on context
            switch(GameManager.State.CurrentGameContext)
            {
                case (GameContext.MainContext):
                    helpButtons[(int)Help.TwoWords].interactable = false;
                    helpButtons[(int)Help.Change].interactable = !GameManager.State.HelpUsed[(int)Help.Change] && CanInteractWithHelp();
                    helpButtons[(int)Help.NextLine].interactable = false;
                    break;
                case (GameContext.CategoryContext):
                    helpButtons[(int)Help.TwoWords].interactable = false;
                    helpButtons[(int)Help.Change].interactable = false;
                    helpButtons[(int)Help.NextLine].interactable = false;
                    break;
                case (GameContext.SongContext):
                    helpButtons[(int)Help.TwoWords].interactable = !GameManager.State.HelpUsed[(int)Help.TwoWords] && CanInteractWithHelp();
                    helpButtons[(int)Help.Change].interactable = false;
                    helpButtons[(int)Help.NextLine].interactable = !GameManager.State.HelpUsed[(int)Help.NextLine] && CanInteractWithHelp();
                    break;
            }
        }

        private void HelpButtonTextsUpdate()
        {
            for (int i = 0; i < Constants.CATEGORY_NUMBER; i++)
                categoryToChangeButtons[i].GetComponentInChildren<Text>().text = SongManager.GetCategoryNameById(i).ToUpper();
        }

        private bool CanInteractWithHelp()
        {
            return !GameManager.OptionsShown && !SongManager.State.IsAnswered && SongManager.IsSongEnded() && !GameManager.IsHitTime();
        }
    }
}
