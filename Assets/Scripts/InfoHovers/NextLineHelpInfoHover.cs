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
                    infoText = "Wy�wietli tre�� kolejnej linii tekstu piosenki";
                else
                {
                    switch (GameManager.CurrentGameContext)
                    {
                        case GameContext.MainContext:
                            infoText = "Musisz wybra� kategori� oraz piosenk�, a ko�o b�dzie aktywne po przes�uchaniu piosenki";
                            break;
                        case GameContext.CategoryContext:
                            infoText = "Musisz wybra� piosenk�, a ko�o b�dzie aktywne po przes�uchaniu piosenki";
                            break;
                        case GameContext.SongContext:
                            infoText = "Ko�o b�dzie aktywne po przes�uchaniu piosenki";
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
