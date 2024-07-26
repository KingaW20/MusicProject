using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Scripts
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] contexts = new GameObject[3];
        [SerializeField] private GameObject[] points = new GameObject[8];
        [SerializeField] private Text mainInfoText;
        [SerializeField] private Button mainInfoButton;

        [Header("Options properties")]
        [SerializeField] private GameObject optionsBox;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button saveGameButton;

        private void Start()
        {
            foreach (var id in Constants.GUARANTEED_SUM_IDS)
                points[id].transform.localScale = new Vector3(1.2f, 1f, 1f);

            for (int i = 0; i < Constants.CATEGORY_NUMBER + 1; i++)
                points[i].GetComponentInChildren<Text>().text = Constants.SUMS[i].ToString();
        }

        void Update()
        {
            foreach (var contextObject in contexts)
                contextObject.SetActive(false);
            contexts[(int)GameManager.State.CurrentGameContext].SetActive(true);

            if (GameManager.State.CurrentGameContext == GameContext.MainContext)
            {
                foreach (Help helpType in Enum.GetValues(typeof(Help)))
                {
                    if (helpType != Help.None)
                        GameManager.State.HelpJustUsed[(int)helpType] = false;
                }
            }

            OptionsUpdate();
            PointsUpdate();
            MainInfoUpdate();
        }

        public void OnOptionShowButtonClick()
        {
            GameManager.OptionsShown = !GameManager.OptionsShown;
        }

        public void OnContinueButtonClick()
        {
            GameManager.OptionsShown = false;
        }

        public void OnSaveButtonClick()
        {
            GameManager.SaveWindowShown = true;
        }

        public void OnExitButtonClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnHitButtonClick()
        {
            SongManager.State.CurrentSongId = Constants.SONG_NUMBER;
            GameManager.State.CurrentGameContext = GameContext.SongContext;
            GameManager.JustChangedToSongContext = true;
            SongManager.State.CurrentLineId = 0;
            GameManager.State.HitSong.RandomizeAnswer();
            SongManager.State.SongSourcePath = GameManager.State.HitSong.GetSongFilePathInResources();
        }

        private void OptionsUpdate()
        {
            optionsBox.SetActive(GameManager.OptionsShown);
            optionsButton.interactable = !GameManager.HelpShown.Any(shown => shown == true) && 
                (GameManager.State.CurrentGameContext != GameContext.SongContext || SongManager.State.IsAnswered || !SongManager.State.IsPlaying);
            saveGameButton.interactable = optionsButton.interactable;
        }

        private void PointsUpdate()
        {
            for (int i = 0; i < GameManager.State.AnswersCorrectness.Count; i++)
            {
                points[i].gameObject.GetComponent<Image>().color = GameManager.State.AnswersCorrectness[i] switch
                {
                    CorrectnessType.Right => Constants.POSITIVE_COLOR,
                    CorrectnessType.AlmostRight => Constants.ALMOST_RIGHT_ANSWER_COLOR,
                    CorrectnessType.Wrong => Constants.NEGATIVE_COLOR
                };
            }
        }

        private void MainInfoUpdate()
        {
            mainInfoButton.interactable = GameManager.State.CurrentGameContext == GameContext.MainContext && GameManager.IsHitTime();

            switch (GameManager.State.CurrentGameContext)
            {
                case GameContext.MainContext:
                    mainInfoText.text = "HIT ZA 50 000";
                    break;
                case GameContext.CategoryContext:
                    mainInfoText.text = SongManager.GetCategoryNameById(SongManager.State.CurrentCategoryId).ToUpper();
                    break;
                case GameContext.SongContext:
                    mainInfoText.text = SongManager.GetCurrentSong().Title.ToUpper();
                    break;
            }
        }
    }
}
