using System.Collections;
using System.Collections.Generic;
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
            if (!GameManager.HelpShown[(int)Help.Change] && !GameManager.HelpUsed[(int)Help.Change])
            {
                changeHelpInfoBox.SetActive(true);
                string infoText = "";

                infoText = "Ko³o aktywne podczas wybierania kategorii\nZamieni wybran¹ przez Ciebie kategoriê\nz inn¹ losowo wybran¹";

                //if (GetComponent<Button>().interactable)
                //    infoText = "Zamieni wybran¹ przez Ciebie kategoriê\nz inn¹ losowo wybran¹";
                //else if (GameManager.CurrentGameContext != GameContext.MainContext)
                //    infoText = "Ko³o bêdzie aktywne podczas wybierania kategorii";

                changeHelpInfoBox.GetComponentInChildren<Text>().text = infoText;
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
