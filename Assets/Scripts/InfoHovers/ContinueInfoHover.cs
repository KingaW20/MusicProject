using UnityEngine;
using UnityEngine.EventSystems;

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
            if (GameManager.ShowOnHover(additionalCondition: this.gameObject.activeSelf))
                continueInfoBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            continueInfoBox.SetActive(false);
        }
    }
}
