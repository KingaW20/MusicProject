using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CategoryContextController : MonoBehaviour
    {
        [SerializeField] private Text[] songTexts = new Text[2];
        [SerializeField] private Button[] songButtons = new Button[2];

        void Update()
        {
            foreach (var songBut in songButtons)
                songBut.interactable = !GameManager.OptionsShown;
        }

        public void OnSongShowButtonClick(int songId)
        {
            GameManager.CurrentSongId = songId;
            GameManager.CurrentGameContext = GameContext.SongContext;
        }
    }
}
