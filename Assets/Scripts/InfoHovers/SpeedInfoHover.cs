using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class SpeedInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private GameObject speedInfoBox;

        void Start()
        {
            speedInfoBox.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpdateSpeedInfoText();
            if (GameManager.ShowOnHover())
                speedInfoBox.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            speedInfoBox.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UpdateSpeedInfoText();
        }

        private void UpdateSpeedInfoText()
        {
            if (SongManager.songFaster)
                speedInfoBox.GetComponentInChildren<Text>().text = "Kliknij, aby spowolnić piosenkę do normalnej prędkości.";
            else
                speedInfoBox.GetComponentInChildren<Text>().text = "Kliknij, aby przyspieszyć piosenkę.";
        }
    }
}
