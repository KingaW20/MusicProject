using Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndContextController : MonoBehaviour
{
    [SerializeField] private Text endInfoLine;
    [SerializeField] private Button menuButton;

    [SerializeField] private Image background;
    [SerializeField] private Sprite basicBackground;
    [SerializeField] private Sprite redBackground;

    void Update()
    {
        if (GameManager.CurrentGameContext == GameContext.EndContext)
        {
            if (GameManager.AnswersCorrectness.Last() && GameManager.IsHitTime())
            {
                endInfoLine.text = "KONIEC GRY\n WYGRA£EŒ 50 000";
                endInfoLine.color = Constants.POSITIVE_COLOR;
                background.sprite = basicBackground;
                background.color = Constants.POSITIVE_COLOR;
                menuButton.GetComponent<Image>().color = Constants.POSITIVE_COLOR;
            }
            else if (!GameManager.AnswersCorrectness.Last())
            {
                endInfoLine.text = "KONIEC GRY\n PRZEGRA£EŒ";
                endInfoLine.color = Constants.NEGATIVE_COLOR;
                background.sprite = redBackground;
                background.color = Constants.NEUTRAL_COLOR;
                menuButton.GetComponent<Image>().color = Constants.NEGATIVE_COLOR;
            }
        }
    }

    public void OnSubmitButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
