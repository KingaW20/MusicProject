using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class TwoWordsHelpInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject twoWordsHelpInfoBox;

        void Start()
        {
            twoWordsHelpInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.ShowOnHover(helpType: Help.TwoWords))
            {
                twoWordsHelpInfoBox.SetActive(true);
                twoWordsHelpInfoBox.GetComponentInChildren<Text>().text = 
                    "Ko�o aktywne przed weryfikacj� odpowiedzi\nWska�e tre�� 2 wybranych przez Ciebie wyraz�w odpowiedzi";
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            twoWordsHelpInfoBox.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            twoWordsHelpInfoBox.SetActive(false);
        }
    }
}
