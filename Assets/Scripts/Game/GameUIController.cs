using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Scripts.GameUIController;

namespace Scripts
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] contexts = new GameObject[3];
        [SerializeField] private GameObject[] points = new GameObject[8];
        [SerializeField] private Text mainInfoText;
        [SerializeField] private Button mainInfoButton;

        [Header("Help properties")]
        [SerializeField] private GameObject[] helpBoxes = new GameObject[Constants.HELP_NUMBER];
        [SerializeField] private Button[] helpButtons = new Button[Constants.HELP_NUMBER];
        [SerializeField] private GameObject[] helpUsedImages = new GameObject[Constants.HELP_NUMBER];

        [SerializeField] private Button[] helpWordButtons = new Button[9];
        [SerializeField] private Button[] categoryToChangeButtons = new Button[Constants.CATEGORY_NUMBER];
        [SerializeField] private Text nextLineText;

        [Header("Options properties")]
        [SerializeField] private GameObject optionsBox;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button saveGameButton;

        private bool[] helpShown = new bool[Constants.HELP_NUMBER];

        public enum Help
        {
            TwoWords,
            Change,
            NextLine
        }

        void Start()
        {
            GameManager.Setup();

            for (int i = 0; i < Constants.HELP_NUMBER; i++)
            {
                helpShown[i] = false;
                helpUsedImages[i].SetActive(GameManager.HelpUsed[i]);
            }

            GameManager.CurrentGameContext = GameContext.MainContext;
            ContentUpdate();
        }

        void Update()
        {
            foreach (var contextObject in contexts)
                contextObject.SetActive(false);
            contexts[(int)GameManager.CurrentGameContext].SetActive(true);
            ContentUpdate();
        }

        #region Help Functions
        public void OnTwoWordsHelpButtonClick()
        {
            for (int i = 0; i < helpWordButtons.Length; i++)
                helpWordButtons[i].gameObject.SetActive(i < GameManager.AnswerWordNumber);
            ShowHelp(Help.TwoWords);
        }

        public void OnChooseWordButtonClick(int wordId)
        {
            GameManager.ChoosedHelpWords.Add(wordId);
            helpWordButtons[wordId].enabled = false;
            helpWordButtons[wordId].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            helpWordButtons[wordId].GetComponentInChildren<Text>().text = GameManager.SongManager.GetWordIdFromCurrentSongAnswer(wordId);

            if (GameManager.ChoosedHelpWords.Count == Constants.HELP_WORDS)
            {
                foreach (var word in helpWordButtons)
                    word.interactable = false;
                GameManager.HelpUsed[(int)Help.TwoWords] = true;
            }
        }

        public void OnChangeHelpButtonClick()
        {
            for (int id = 0; id < Constants.CATEGORY_NUMBER; id++)
                categoryToChangeButtons[id].interactable = !GameManager.ChoosedCategories.Contains(id);
            ShowHelp(Help.Change);
        }

        public void OnChooseCategoryToChangeButtonClick(int categoryId)
        {
            foreach (var cat in categoryToChangeButtons)
                cat.interactable = false;
            categoryToChangeButtons[categoryId].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            GameManager.HelpUsed[(int)Help.Change] = true;
            GameManager.SongManager.ChangeCategory(categoryId);
        }

        public void OnNextLineHelpButtonClick()
        {
            nextLineText.text = GameManager.SongManager.GetCurrentSong().NextLine;
            ShowHelp(Help.NextLine);
            GameManager.HelpUsed[(int)Help.NextLine] = true;
        }

        private void ShowHelp(Help help)
        {
            helpShown[(int)help] = !helpShown[(int)help];
            helpBoxes[(int)help].SetActive(helpShown[(int)help]);
            HelpButtonsInteractivityUpdate();
        }

        private void HelpButtonsInteractivityUpdate()
        {
            for (int i = 0; i < Constants.HELP_NUMBER; i++)
            {
                helpUsedImages[i].SetActive(GameManager.HelpUsed[i] && !helpBoxes[i].activeSelf);
            }

            // if any of helps is shown set only button of this help interactable
            if (helpShown.Any(item => item == true))
            {
                for (int i = 0; i < Constants.HELP_NUMBER; i++)
                    helpButtons[i].interactable = helpShown[i] && GameManager.HelpUsed[i] && CanInteractWithHelp();
                return;
            }

            // make buttons interactable based on context
            if (GameManager.CurrentGameContext == GameContext.MainContext)
            {
                helpButtons[(int)Help.TwoWords].interactable = false;
                helpButtons[(int)Help.Change].interactable = !GameManager.HelpUsed[(int)Help.Change] && CanInteractWithHelp();
                helpButtons[(int)Help.NextLine].interactable = false;
            }
            else if (GameManager.CurrentGameContext == GameContext.SongContext)
            {
                helpButtons[(int)Help.TwoWords].interactable = !GameManager.HelpUsed[(int)Help.TwoWords] && CanInteractWithHelp();
                helpButtons[(int)Help.Change].interactable = false;
                helpButtons[(int)Help.NextLine].interactable = !GameManager.HelpUsed[(int)Help.NextLine] && CanInteractWithHelp();
            }
        }

        private void HelpButtonTextsUpdate()
        {
            for (int i = 0; i < Constants.CATEGORY_NUMBER; i++)
                categoryToChangeButtons[i].GetComponentInChildren<Text>().text = GameManager.SongManager.GetCategoryNameById(i);
        }

        private bool CanInteractWithHelp()
        {
            return !GameManager.OptionsShown && !GameManager.SongManager.IsAnswered && 
                GameManager.SongManager.IsSongEnded() && !GameManager.SongManager.IsHit();
        }
        #endregion

        #region Options
        public void OnOptionShowButtonClick()
        {
            GameManager.OptionsShown = !GameManager.OptionsShown;
            OptionsContentUpdate();
        }

        public void OnContinueButtonClick()
        {
            GameManager.OptionsShown = false;
            OptionsContentUpdate();
        }

        public void OnSaveButtonClick()
        {

        }

        public void OnExitButtonClick()
        {
            SceneManager.LoadScene("Menu");
        }

        private void OptionsContentUpdate()
        {
            optionsBox.SetActive(GameManager.OptionsShown);
            saveGameButton.interactable = false;
        }

        private void OptionsButtonInteractivityUpdate()
        {
            optionsButton.interactable = !helpShown.Any(item => item == true) && 
                ((GameManager.SongManager.IsSongEnded() && GameManager.SongManager.IsAnswered) ||
                GameManager.SongManager.CurrentTime <= 0);
        }
        #endregion

        private void ContentUpdate()
        {
            OptionsContentUpdate();
            OptionsButtonInteractivityUpdate();
            HelpButtonTextsUpdate();
            HelpButtonsInteractivityUpdate();
            PointsUpdate();
            MainInfoUpdate();
            for (int i = 0; i < Constants.HELP_NUMBER; i++)
                helpBoxes[i].SetActive(helpShown[i]);
        }

        public void OnHitButtonClick()
        {
            GameManager.SongManager.CurrentSongId = Constants.SONG_NUMBER;         //to sprawdziæ wszêdzie
            GameManager.CurrentGameContext = GameContext.SongContext;
            GameManager.SongManager.CurrentLineId = 0;
            GameManager.SongManager.HitSong.RandomizeAnswer();
            GameManager.SongManager.SongSourcePath = GameManager.SongManager.HitSongFilePath;
        }

        private void MainInfoUpdate()
        {
            mainInfoButton.interactable = GameManager.CurrentGameContext == GameContext.MainContext && GameManager.SongManager.IsHit();

            switch (GameManager.CurrentGameContext)
            {
                case GameContext.MainContext:
                    mainInfoText.text = "HIT ZA 50 000";
                    break;
                case GameContext.CategoryContext:
                    mainInfoText.text = GameManager.SongManager.GetCategoryNameById(GameManager.SongManager.CurrentCategoryId);
                    break;
                case GameContext.SongContext:
                    mainInfoText.text = GameManager.SongManager.GetCurrentSong().Title.ToUpper();
                    break;
            }
        }

        private void PointsUpdate()
        {
            for (int i = 0; i < GameManager.CorrectAnswers; i++)
                points[i].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            if (!GameManager.LastAnswerCorrect && GameManager.CorrectAnswers < Constants.CATEGORY_NUMBER)
                points[GameManager.CorrectAnswers].gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f);
        }
    }
}
