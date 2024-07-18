using Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndContextController : MonoBehaviour
{
    [SerializeField] private Text endInfoLine;
    [SerializeField] private Button submitButton;

    [SerializeField] private Image background;
    [SerializeField] private Sprite basicBackground;
    [SerializeField] private Sprite redBackground;

    void Update()
    {
        if (GameManager.State.CurrentGameContext == GameContext.EndContext)
        {
            var win = GameManager.State.AnswersCorrectness.Last() != CorrectnessType.Wrong;

            endInfoLine.text = win ? "Koniec gry\n WYGRA£EŒ 50 000\nGratulacje!!!" : "Koniec gry\n PRZEGRA£EŒ";
            endInfoLine.color = win ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
            background.sprite = win ? basicBackground : redBackground;
            background.color = win ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
            submitButton.GetComponent<Image>().color = win ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
        }
    }

    public void OnSubmitButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
