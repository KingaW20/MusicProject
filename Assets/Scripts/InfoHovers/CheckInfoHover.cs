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
                    infoText = "Sprawd� poprawno�� odpowiedzi";
                else if (SongManager.IsSongEnded())
                    infoText = "Odpowied� jest za d�uga\nmaksymalna liczba s��w: " + GameManager.State.AnswerWordNumber;
                else
                    infoText = "Sprawdzenie odpowiedzi b�dzie mo�liwe po przes�uchaniu piosenki";

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
