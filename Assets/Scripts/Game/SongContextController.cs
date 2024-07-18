using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class SongContextController : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Sprite basicBackground;
        [SerializeField] private Sprite redBackground;
        [SerializeField] private Button mainInfoButton;

        [SerializeField] private GameObject songLineBackground;
        [SerializeField] private GameObject nextSongLineBackground;
        [SerializeField] private Text songLine;
        [SerializeField] private Text nextSongLine;
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
            answer.gameObject.SetActive(GameManager.State.CurrentGameContext == GameContext.SongContext);

            if (GameManager.State.CurrentGameContext == GameContext.SongContext)
            {
                if (GameManager.GameLoadedOnSong)
                    UpdateViewState();

                wordNumberText.text = "Liczba s³ów: " + GameManager.State.AnswerWordNumber;

                if (songSource.isPlaying)
                {
                    SongManager.State.CurrentTime = songSource.time;
                    songLine.text = GameManager.JustChangedToSongContext ? "Tekst piosenki" : SongManager.GetCurrentLine();
                    nextSongLine.text = GameManager.JustChangedToSongContext ? "Tekst piosenki" : SongManager.GetNextLine();
                }
                answer.interactable = SongManager.IsSongEnded();

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

                if (GameManager.JustChangedToSongContext)
                {
                    playButton.interactable = true;
                    songLine.text = "Tekst piosenki";
                    nextSongLine.text = "Tekst piosenki";
                }

                if (GameManager.State.EnteredAnswer != answer.text)
                    GameManager.State.EnteredAnswer = answer.text;
            }
        }

        public void OnPlayAndPauseButtonClick()
        {
            if (songSource.clip == null)
                songSource.clip = Resources.Load<AudioClip>(SongManager.State.SongSourcePath);

            if (!songSource.isPlaying)
            {
                songSource.Play();
                GameManager.JustChangedToSongContext = false;
                SongManager.State.IsPlaying = true;
            }
            else
            {
                songSource.Pause();
                SongManager.State.IsPlaying = false;
            }

            playImage.gameObject.SetActive(!songSource.isPlaying);
            pauseImage.gameObject.SetActive(songSource.isPlaying);
        }

        public void OnCheckAnswerButtonClick()
        {
            SongManager.State.IsAnswered = !SongManager.State.IsAnswered;

            if (SongManager.State.IsAnswered)
            {
                var correct = SongManager.IsAnswerCorrect(answer.text);
                GameManager.State.AnswersCorrectness.Add(correct ? CorrectnessType.Right : CorrectnessType.Wrong);
                ChangeButtonColor(correct ? Constants.POSITIVE_COLOR : Constants.NEUTRAL_COLOR, correct);
                if (!correct)
                    rightAnswerText.text = SongManager.GetCurrentSong().Answer;
            }
            if (!SongManager.State.IsAnswered)
            {
                songSource.clip = null;
                SongManager.State.SongSourcePath = "";
                GameManager.State.AnswerWordNumber++;
                answer.text = "";
                ChangeButtonColor(Constants.NEUTRAL_COLOR, true);
                bool lastCorrect = GameManager.State.AnswersCorrectness.Last() != CorrectnessType.Wrong;
                if ((lastCorrect && GameManager.State.AnswersCorrectness.Count > Constants.CATEGORY_NUMBER) || !lastCorrect)
                    GameManager.State.CurrentGameContext = GameContext.EndContext;
                else if (lastCorrect)
                    GameManager.State.CurrentGameContext = GameContext.MainContext;
            }

            SongContextButtonsInteractivityUpdate();
        }

        public void OnContinueButtonClick()
        {
            GameManager.State.AnswersCorrectness[GameManager.State.AnswersCorrectness.Count - 1] = CorrectnessType.AlmostRight;
            ChangeButtonColor(Constants.POSITIVE_COLOR, true);
        }

        private void UpdateViewState()
        {
            songSource.clip = Resources.Load<AudioClip>(SongManager.State.SongSourcePath);
            songSource.time = SongManager.State.CurrentTime;

            songLine.text = SongManager.GetCurrentLine();
            nextSongLine.text = SongManager.GetNextLine();
            answer.text = GameManager.State.EnteredAnswer;

            GameManager.GameLoadedOnSong = false;

            if (SongManager.State.IsAnswered)
            {
                bool correct = GameManager.State.AnswersCorrectness.Last() != CorrectnessType.Wrong;
                ChangeButtonColor(correct ? Constants.POSITIVE_COLOR : Constants.NEUTRAL_COLOR, correct);
                if (!correct)
                    rightAnswerText.text = SongManager.GetCurrentSong().Answer;
                SongContextButtonsInteractivityUpdate();
            }
        }

        private void ChangeButtonColor(Color color, bool correct)
        {
            this.background.sprite = correct ? basicBackground : redBackground;
            this.background.color = color;
            songLineBackground.GetComponent<Image>().color = color;
            nextSongLineBackground.GetComponent<Image>().color = color;
            mainInfoButton.GetComponent<Image>().color = color;
            answer.image.color = color;
            playButton.GetComponent<Image>().color = color;
            checkButton.GetComponent<Image>().color = color;
            rightAnswerBox.SetActive(!correct);
        }

        private void SongContextButtonsInteractivityUpdate()
        {
            var correct = GameManager.State.AnswersCorrectness.Last() != CorrectnessType.Wrong;

            if (SongManager.State.IsAnswered)
                checkButtonText.text = "Wybierz kolejn¹ kategoriê";
            else if (correct && GameManager.State.AnswersCorrectness.Count > Constants.CATEGORY_NUMBER)
                checkButtonText.text = "Wygra³eœ! Zakoñcz grê";
            else if (!correct)
                checkButtonText.text = "Zakoñcz grê";
            else
                checkButtonText.text = "TAK TO LECIA£O!";
        }
    }
}
