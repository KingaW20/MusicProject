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
                    "Ko³o aktywne przed weryfikacj¹ odpowiedzi\nWska¿e treœæ 2 wybranych przez Ciebie wyrazów odpowiedzi";
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
