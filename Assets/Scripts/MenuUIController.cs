using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            contexts[(int)GameManager.CurrentMenuContext].SetActive(true);
        }

        public void OnPlayButtonClick()
        {
            GameManager.CurrentMenuContext = MenuContext.PlayContext;
        }

        public void OnLoadButtonClick()
        {
            GameManager.CurrentMenuContext = MenuContext.LoadContext;
        }
    }
}
