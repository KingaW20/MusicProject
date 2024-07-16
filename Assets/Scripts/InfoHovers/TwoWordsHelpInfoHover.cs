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

                infoText = "Ko³o aktywne przed weryfikacj¹ odpowiedzi\nWska¿e treœæ 2 wybranych przez Ciebie wyrazów odpowiedzi";

                //if (GetComponent<Button>().interactable)
                //    infoText = "Wska¿e treœæ 2 wybranych przez Ciebie wyrazów odpowiedzi";
                //else
                //{
                //    switch (GameManager.CurrentGameContext)
                //    {
                //        case GameContext.MainContext:
                //            infoText = "Musisz wybraæ kategoriê oraz piosenkê, a ko³o bêdzie aktywne po przes³uchaniu piosenki";
                //            break;
                //        case GameContext.CategoryContext:
                //            infoText = "Musisz wybraæ piosenkê, a ko³o bêdzie aktywne po przes³uchaniu piosenki";
                //            break;
                //        case GameContext.SongContext:
                //            infoText = "Ko³o bêdzie aktywne po przes³uchaniu piosenki";
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
