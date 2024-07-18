using UnityEngine;

namespace Scripts
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] contexts = new GameObject[3];

        void Awake()
        {
            GameManager.Setup();
        }

        void Update()
        {
            foreach (var contextObject in contexts)
                contextObject.SetActive(false);
            contexts[(int)GameManager.State.CurrentMenuContext].SetActive(true);
        }

        public void OnPlayButtonClick()
        {
            GameManager.State.CurrentMenuContext = MenuContext.PlayContext;
        }

        public void OnLoadButtonClick()
        {
            GameManager.State.CurrentMenuContext = MenuContext.LoadContext;
        }

        public void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}
