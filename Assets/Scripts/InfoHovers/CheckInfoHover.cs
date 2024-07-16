using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class CheckInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject checkInfoBox;

        void Start()
        {
            checkInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.ShowOnHover(additionalCondition: GameManager.State.AnswersCorrectness.Count < GameManager.State.ChoosedCategoryIds.Count))
            {
                string infoText = "";
                checkInfoBox.SetActive(true);

                if (GetComponent<Button>().interactable)
                    infoText = "SprawdŸ poprawnoœæ odpowiedzi";
                else if (SongManager.IsSongEnded())
                    infoText = "OdpowiedŸ jest za d³uga\nmaksymalna liczba s³ów: " + GameManager.State.AnswerWordNumber;
                else
                    infoText = "Sprawdzenie odpowiedzi bêdzie mo¿liwe po przes³uchaniu piosenki";

                checkInfoBox.GetComponentInChildren<Text>().text = infoText;
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            checkInfoBox.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            checkInfoBox.SetActive(false);
        }
    }
}
