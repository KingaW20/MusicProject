using System.Collections;
using System.Collections.Generic;
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
            if (!GameManager.HelpShown[(int)Help.TwoWords] && !GameManager.HelpUsed[(int)Help.TwoWords])
            {
                twoWordsHelpInfoBox.SetActive(true);
                string infoText = "";

                infoText = "Ko�o aktywne przed weryfikacj� odpowiedzi\nWska�e tre�� 2 wybranych przez Ciebie wyraz�w odpowiedzi";

                //if (GetComponent<Button>().interactable)
                //    infoText = "Wska�e tre�� 2 wybranych przez Ciebie wyraz�w odpowiedzi";
                //else
                //{
                //    switch (GameManager.CurrentGameContext)
                //    {
                //        case GameContext.MainContext:
                //            infoText = "Musisz wybra� kategori� oraz piosenk�, a ko�o b�dzie aktywne po przes�uchaniu piosenki";
                //            break;
                //        case GameContext.CategoryContext:
                //            infoText = "Musisz wybra� piosenk�, a ko�o b�dzie aktywne po przes�uchaniu piosenki";
                //            break;
                //        case GameContext.SongContext:
                //            infoText = "Ko�o b�dzie aktywne po przes�uchaniu piosenki";
                //            break;
                //    }
                //}

                twoWordsHelpInfoBox.GetComponentInChildren<Text>().text = infoText;
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
