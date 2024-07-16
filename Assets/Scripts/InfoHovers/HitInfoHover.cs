using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class HitInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject hitInfoBox;

        void Start()
        {
            hitInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.ShowOnHover(additionalCondition: 
                !this.gameObject.GetComponent<Button>().interactable && GameManager.State.CurrentGameContext == GameContext.MainContext))
                hitInfoBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hitInfoBox.SetActive(false);
        }
    }
}
