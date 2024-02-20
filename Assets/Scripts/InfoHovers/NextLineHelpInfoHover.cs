using System.Collections;
using System.Collections.Generic;
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
            if (!GameManager.HelpShown[(int)Help.NextLine] && !GameManager.HelpUsed[(int)Help.NextLine])
            {
                nextLineHelpInfoBox.SetActive(true);
                string infoText = "";

                if (GetComponent<Button>().interactable)
                    infoText = "Wyœwietli treœæ kolejnej linii tekstu piosenki";
                else
                {
                    switch (GameManager.CurrentGameContext)
                    {
                        case GameContext.MainContext:
                            infoText = "Musisz wybraæ kategoriê oraz piosenkê, a ko³o bêdzie aktywne po przes³uchaniu piosenki";
                            break;
                        case GameContext.CategoryContext:
                            infoText = "Musisz wybraæ piosenkê, a ko³o bêdzie aktywne po przes³uchaniu piosenki";
                            break;
                        case GameContext.SongContext:
                            infoText = "Ko³o bêdzie aktywne po przes³uchaniu piosenki";
                            break;
                    }
                }

                nextLineHelpInfoBox.GetComponentInChildren<Text>().text = infoText;
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
