using System.Collections;
using System.Collections.Generic;
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

                GameManager.SongManager.CurrentTime = songSource.time;
                answer.interactable = GameManager.SongManager.IsSongEnded();
                songLine.text = GameManager.SongManager.GetCurrentLine();

                if (!GameManager.SongManager.IsSongEnded())
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
                songSource.clip = Resources.Load<AudioClip>(GameManager.SongManager.SongSourcePath);
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
            GameManager.SongManager.IsAnswered = !GameManager.SongManager.IsAnswered;

            //TODO: tu coú jest nie tak, bo nie przechodzi dobrze
            if (GameManager.SongManager.IsAnswered)
            {
                bool correct = GameManager.SongManager.IsAnswerCorrect(answer.text);
                if (correct) GameManager.CorrectAnswers++;
                background.sprite = correct ? basicBackground : redBackground;
                background.color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 1f, 1f);
                songLineBackground.GetComponent<Image>().color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                mainInfoButton.GetComponent<Image>().color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                answer.image.color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                playButton.GetComponent<Image>().color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                checkButton.GetComponent<Image>().color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                GameManager.LastAnswerCorrect = correct;
                if (!correct)
                {
                    rightAnswerBox.SetActive(true);
                    rightAnswerText.text = GameManager.SongManager.GetCurrentSongAnswer().ToUpper();
                }
            }
            if (!GameManager.SongManager.IsAnswered)
            {
                songSource.clip = null;
                GameManager.SongManager.SongSourcePath = "";

                GameManager.AnswerWordNumber++;
                answer.text = "";
                background.sprite = basicBackground;
                background.color = new Color(1f, 1f, 1f);
                songLineBackground.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                mainInfoButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                answer.image.color = new Color(1f, 1f, 1f);
                playButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                checkButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                rightAnswerBox.SetActive(false);
                if (GameManager.LastAnswerCorrect)
                    GameManager.CurrentGameContext = GameContext.MainContext;
                else
                    SceneManager.LoadScene("Menu");
            }

            SongContextButtonsInteractivityUpdate();
        }

        private void SongContextButtonsInteractivityUpdate()
        {
            checkButtonText.text = GameManager.SongManager.IsAnswered ? "Wybierz kolejn• kategori " : "Tak to lecia£o!";
            checkButtonText.text = GameManager.LastAnswerCorrect && 
                GameManager.CorrectAnswers > Constants.CATEGORY_NUMBER ? "Wygra£eå! Zako—cz gr " : checkButtonText.text;
            checkButtonText.text = GameManager.LastAnswerCorrect ? checkButtonText.text : "Zako—cz gr ";
        }
    }
}
