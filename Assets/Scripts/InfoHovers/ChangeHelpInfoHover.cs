using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class ChangeHelpInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject changeHelpInfoBox;

        void Start()
        {
            changeHelpInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.ShowOnHover(helpType: Help.Change))
            {
                changeHelpInfoBox.SetActive(true);
                changeHelpInfoBox.GetComponentInChildren<Text>().text = 
                    "Ko�o aktywne podczas wybierania kategorii\nZamieni wybran� przez Ciebie kategori�\nz inn� losowo wybran�";
            }
            else if (GameManager.State.HelpUsed[(int)Help.Change] && !GameManager.HelpShown[(int)Help.Change])
            {
                changeHelpInfoBox.SetActive(true);
                changeHelpInfoBox.GetComponentInChildren<Text>().text = GameManager.State.ChangeInfoText;
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            changeHelpInfoBox.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            changeHelpInfoBox.SetActive(false);
        }
    }
}
