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
        [SerializeField] private Text songLine;
        [SerializeField] private InputField answer;
        [SerializeField] private Button playButton;
        [SerializeField] private Image playImage;
        [SerializeField] private Image pauseImage;
        [SerializeField] private Text wordNumberText;
        [SerializeField] private Button checkButton;
        [SerializeField] private Text checkButtonText;

        private bool resultShown;
        private bool play;

        void Start()
        {
            resultShown = false;
            play = true;
        }

        void Update()
        {
            wordNumberText.text = "Liczba s≥Ûw: " + GameManager.AnswerWordNumber;
            answer.gameObject.SetActive(GameManager.CurrentGameContext == GameContext.SongContext);

            /*
            if music plays (didn't end)
            {
                playButton.interactable = !GameManager.OptionsShown;
                checkButton.interactable = false;
            }
            else if music ended
            {
                playButton.interactable = false;
                play = true;
                playImage.gameObject.SetActive(play);
                pauseImage.gameObject.SetActive(!play);
                if answer is not empty
                    checkButton.interactable = !GameManager.OptionsShown;
            }
            */
        }

        public void OnPlayAndPauseButtonClick()
        {
            play = !play;
            playImage.gameObject.SetActive(play);
            pauseImage.gameObject.SetActive(!play);

            if (play)
            {
                //TODO: play the music
            }
            else
            {
                //TODO: pause the music
            }
        }

        public void OnCheckAnswerButtonClick()
        {
            resultShown = !resultShown;

            if (resultShown)
            {
                bool correct = answer.text.ToLower() == GameManager.CorrectAnswer.ToLower();
                if (correct) GameManager.CorrectAnswers++;
                background.color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0.524f);
                answer.image.color = correct ? new Color(0f, 1f, 0f) : new Color(1f, 0f, 0f);
                GameManager.LastAnswerCorrect = correct;
            }
            if (!resultShown)
            {
                GameManager.AnswerWordNumber++;
                answer.text = "";
                background.color = new Color(1f, 1f, 1f);
                answer.image.color = new Color(1f, 1f, 1f);
                if (GameManager.LastAnswerCorrect)
                    GameManager.CurrentGameContext = GameContext.MainContext;
                else
                    SceneManager.LoadScene("Menu");
            }

            SongContextButtonsInteractivityUpdate();
        }

        private void SongContextButtonsInteractivityUpdate()
        {
            checkButtonText.text = resultShown ? "Wybierz kolejn• kategori " : "Tak to lecia£o!";
            checkButtonText.text = GameManager.LastAnswerCorrect ? checkButtonText.text : "Zako—cz gr ";
        }
    }
}
