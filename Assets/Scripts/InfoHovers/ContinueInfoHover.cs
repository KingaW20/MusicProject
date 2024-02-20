using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class ContinueInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject continueInfoBox;

        void Start()
        {
            continueInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.gameObject.activeSelf)
                continueInfoBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            continueInfoBox.SetActive(false);
        }
    }
}
