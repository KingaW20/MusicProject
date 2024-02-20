using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject playInfoBox;

    void Start()
    {
        playInfoBox.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!this.gameObject.GetComponent<Button>().interactable)
            playInfoBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        playInfoBox.SetActive(false);
    }
}
