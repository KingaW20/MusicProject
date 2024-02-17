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
        [SerializeField] private GameObject[] helpBoxes = new GameObject[3];
        [SerializeField] private Button[] helpButtons = new Button[3];
        [SerializeField] private GameObject[] helpUsedImages = new GameObject[3];
        [SerializeField] private Text nextLineText;

        [SerializeField] private Button[] helpWordButtons = new Button[9];
        [SerializeField] private Button[] categoryToChangeButtons = new Button[7];

        [Header("Options properties")]
        [SerializeField] private GameObject optionsBox;
        [SerializeField] private Button saveGameButton;

        private bool[] helpShown = new bool[3];

        public enum Help
        {
            TwoWords,
            Change,
            NextLine
        }

        void Start()
        {
            GameManager.Setup();

            for (int i = 0; i < helpShown.Length; i++)
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
            ShowHelp(Help.TwoWords);
        }

        public void OnChooseWordButtonClick(int wordId)
        {
            GameManager.ChoosedHelpWords.Add(wordId);
            helpWordButtons[wordId].enabled = false;
            helpWordButtons[wordId].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            //TODO: show this word from answer in helpWordButtons[wordId].GetChild(0).GetComponent<Text>().text

            if (GameManager.ChoosedHelpWords.Count == 2)
            {
                foreach (var word in helpWordButtons)
                    word.interactable = false;
                GameManager.HelpUsed[(int)Help.TwoWords] = true;
            }
        }

        public void OnChangeHelpButtonClick()
        {
            ShowHelp(Help.Change);
        }

        public void OnChooseCategoryToChangeButtonClick(int categoryId)
        {
            GameManager.ChoosedCategoryToChange = categoryId;

            foreach (var cat in categoryToChangeButtons)
                cat.interactable = false;
            categoryToChangeButtons[categoryId].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            GameManager.HelpUsed[(int)Help.Change] = true;

            //TODO: actual change of category
        }

        public void OnNextLineHelpButtonClick()
        {
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
            for (int i = 0; i < helpButtons.Length; i++)
            {
                helpButtons[i].interactable = false;
                helpUsedImages[i].SetActive(GameManager.HelpUsed[i] && !helpBoxes[i].activeSelf);
            }

            // if any of helps is shown set only button of this help interactable
            if (helpShown.Any(item => item == true))
            {
                for (int i = 0; i < helpButtons.Length; i++)
                    helpButtons[i].interactable = helpShown[i] && GameManager.HelpUsed[i] && CanInteractWithHelp();
                return;
            }

            // make buttons interactable based on context
            if (GameManager.CurrentGameContext == GameContext.MainContext)
            {
                helpButtons[(int)Help.Change].interactable = !GameManager.HelpUsed[(int)Help.Change] && CanInteractWithHelp();
            }
            else if (GameManager.CurrentGameContext == GameContext.SongContext)
            {
                helpButtons[(int)Help.TwoWords].interactable = !GameManager.HelpUsed[(int)Help.TwoWords] && CanInteractWithHelp();
                helpButtons[(int)Help.NextLine].interactable = !GameManager.HelpUsed[(int)Help.NextLine] && CanInteractWithHelp();
            }
        }

        private bool CanInteractWithHelp()
        {
            return !GameManager.OptionsShown && !GameManager.Answered;
        }
        #endregion

        #region Options
        public void OnOptionShowButtonClick()
        {
            if (!helpShown.Any(item => item == true))
            {
                GameManager.OptionsShown = !GameManager.OptionsShown;
                OptionsContentUpdate();
            }
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
        #endregion

        private void ContentUpdate()
        {
            OptionsContentUpdate();
            HelpButtonsInteractivityUpdate();
            PointsUpdate();
            for (int i = 0; i < helpBoxes.Length; i++)
                helpBoxes[i].SetActive(helpShown[i]);
        }

        private void PointsUpdate()
        {
            for (int i = 0; i < GameManager.CorrectAnswers; i++)
                points[i].gameObject.GetComponent<Image>().color = new Color(0f, 1f, 0f);
            if (!GameManager.LastAnswerCorrect && GameManager.CorrectAnswers < 7)
                points[GameManager.CorrectAnswers].gameObject.GetComponent<Image>().color = new Color(1f, 0f, 0f);
        }
    }
}
