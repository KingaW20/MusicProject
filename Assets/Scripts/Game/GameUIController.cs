using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        void Update()
        {
            foreach (var contextObject in contexts)
                contextObject.SetActive(false);
            contexts[(int)GameManager.CurrentGameContext].SetActive(true);

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

        }

        public void OnExitButtonClick()
        {
            SceneManager.LoadScene("Menu");
        }

        public void OnHitButtonClick()
        {
            SongManager.CurrentSongId = Constants.SONG_NUMBER;
            GameManager.CurrentGameContext = GameContext.SongContext;
            GameManager.JustChangedToSongContext = true;
            SongManager.CurrentLineId = 0;
            GameManager.HitSong.RandomizeAnswer();
            SongManager.SongSourcePath = GameManager.HitSong.GetSongFilePathInResources();
        }

        private void OptionsUpdate()
        {
            optionsBox.SetActive(GameManager.OptionsShown);
            optionsButton.interactable = !GameManager.HelpShown.Any(shown => shown == true) &&
                (SongManager.IsAnswered || SongManager.CurrentTime <= 0);

            //TODO: interactable = true if saving implemented
            saveGameButton.interactable = false;
        }

        private void PointsUpdate()
        {
            for (int i = 0; i < GameManager.AnswersCorrectness.Count; i++)
                points[i].gameObject.GetComponent<Image>().color = GameManager.AnswersCorrectness[i] ?
                    Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
        }

        private void MainInfoUpdate()
        {
            mainInfoButton.interactable = GameManager.CurrentGameContext == GameContext.MainContext && GameManager.IsHitTime();

            switch (GameManager.CurrentGameContext)
            {
                case GameContext.MainContext:
                    mainInfoText.text = "HIT ZA 50 000";
                    break;
                case GameContext.CategoryContext:
                    mainInfoText.text = SongManager.GetCategoryNameById(SongManager.CurrentCategoryId).ToUpper();
                    break;
                case GameContext.SongContext:
                    mainInfoText.text = SongManager.GetCurrentSong().Title.ToUpper();
                    break;
            }
        }
    }
}
