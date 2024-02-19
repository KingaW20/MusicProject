using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SongContextController : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Sprite basicBackground;
        [SerializeField] private Sprite redBackground;
        [SerializeField] private Button mainInfoButton;

        [SerializeField] private GameObject songLineBackground;
        [SerializeField] private Text songLine;
        [SerializeField] private InputField answer;

        [SerializeField] private Button playButton;
        [SerializeField] private Image playImage;
        [SerializeField] private Image pauseImage;
        [SerializeField] private AudioSource songSource;

        [SerializeField] private Text wordNumberText;
        [SerializeField] private Button checkButton;
        [SerializeField] private Text checkButtonText;

        [SerializeField] private GameObject rightAnswerBox;
        [SerializeField] private Text rightAnswerText;

        void Start()
        {
            rightAnswerBox.SetActive(false);
        }

        void Update()
        {
            answer.gameObject.SetActive(GameManager.CurrentGameContext == GameContext.SongContext);

            if (GameManager.CurrentGameContext == GameContext.SongContext)
            { 
                wordNumberText.text = "Liczba s≥Ûw: " + GameManager.AnswerWordNumber;

                SongManager.CurrentTime = songSource.time;
                answer.interactable = SongManager.IsSongEnded();
                songLine.text = SongManager.GetCurrentLine();

                if (!SongManager.IsSongEnded())
                {
                    playButton.interactable = !GameManager.OptionsShown;
                    checkButton.interactable = false;
                }
                else
                {
                    songSource.Pause();
                    playButton.interactable = false;
                    playImage.gameObject.SetActive(true);
                    pauseImage.gameObject.SetActive(false);
                    if (GameManager.IsAnswerTooLong(answer.text))
                        checkButton.interactable = false;
                    else
                        checkButton.interactable = !GameManager.OptionsShown;
                }
            }
        }

        public void OnPlayAndPauseButtonClick()
        {
            if (songSource.clip == null)
            {
                songSource.clip = Resources.Load<AudioClip>(SongManager.SongSourcePath);
                Debug.Log(songSource.clip);
            }

            if (!songSource.isPlaying)
            {
                songSource.Play();
            }
            else
            {
                songSource.Pause();
            }

            playImage.gameObject.SetActive(!songSource.isPlaying);
            pauseImage.gameObject.SetActive(songSource.isPlaying);
        }

        public void OnCheckAnswerButtonClick()
        {
            SongManager.IsAnswered = !SongManager.IsAnswered;

            if (SongManager.IsAnswered)
            {
                bool correct = SongManager.IsAnswerCorrect(answer.text);
                GameManager.AnswersCorrectness.Add(correct);
                ChangeButtonColor(correct ? Constants.POSITIVE_COLOR : Constants.NEUTRAL_COLOR, correct);
                if (!correct)
                    rightAnswerText.text = SongManager.GetCurrentSong().Answer.ToUpper();
            }
            if (!SongManager.IsAnswered)
            {
                songSource.clip = null;
                SongManager.SongSourcePath = "";
                GameManager.AnswerWordNumber++;
                answer.text = "";
                ChangeButtonColor(Constants.NEUTRAL_COLOR, true);
                if ((GameManager.AnswersCorrectness.Last() && GameManager.AnswersCorrectness.Count > Constants.CATEGORY_NUMBER) 
                    || !GameManager.AnswersCorrectness.Last())
                    SceneManager.LoadScene("Menu");
                else if (GameManager.AnswersCorrectness.Last())
                    GameManager.CurrentGameContext = GameContext.MainContext;

            }

            SongContextButtonsInteractivityUpdate();
        }

        private void ChangeButtonColor(Color color, bool correct)
        {
            this.background.sprite = correct ? basicBackground : redBackground;
            this.background.color = color;
            songLineBackground.GetComponent<Image>().color = color;
            mainInfoButton.GetComponent<Image>().color = color;
            answer.image.color = color;
            playButton.GetComponent<Image>().color = color;
            checkButton.GetComponent<Image>().color = color;
            rightAnswerBox.SetActive(!correct);
        }

        private void SongContextButtonsInteractivityUpdate()
        {
            checkButtonText.text = SongManager.IsAnswered ? "Wybierz kolejn• kategori " : "Tak to lecia£o!";
            checkButtonText.text = 
                GameManager.AnswersCorrectness.Last() && GameManager.AnswersCorrectness.Count > Constants.CATEGORY_NUMBER 
                ? "Wygra£eå! Zako—cz gr " : checkButtonText.text;
            checkButtonText.text = GameManager.AnswersCorrectness.Last() ? checkButtonText.text : "Zako—cz gr ";
        }
    }
}
