using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class NextLineHelpInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject nextLineHelpInfoBox;

        void Start()
        {
            nextLineHelpInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.ShowOnHover(helpType: Help.NextLine))
            {
                nextLineHelpInfoBox.SetActive(true);
                nextLineHelpInfoBox.GetComponentInChildren<Text>().text = 
                    "Ko�o aktywne przed weryfikacj� odpowiedzi\nWy�wietli tre�� kolejnej linii tekstu piosenki";
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            nextLineHelpInfoBox.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            nextLineHelpInfoBox.SetActive(false);
        }
    }
}
