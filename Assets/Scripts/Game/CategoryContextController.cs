using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CategoryContextController : MonoBehaviour
    {
        [SerializeField] private Text[] songTexts = new Text[Constants.SONG_NUMBER];
        [SerializeField] private Button[] songButtons = new Button[Constants.SONG_NUMBER];

        void Update()
        {
            if (GameManager.CurrentGameContext == GameContext.CategoryContext)
            {
                foreach (var songBut in songButtons)
                    songBut.interactable = !GameManager.OptionsShown;
                for (int i = 0; i < Constants.SONG_NUMBER; i++)
                    songTexts[i].text = SongManager.GetCurrentCatSongTitleById(i).ToUpper();
            }
        }

        public void OnSongShowButtonClick(int songId)
        {
            SongManager.CurrentSongId = songId;
            SongManager.CurrentLineId = 0;
            SongManager.GetCurrentSong().RandomizeAnswer();
            SongManager.SongSourcePath = SongManager.GetCurrentSong().GetSongFilePathInResources();
            GameManager.CurrentGameContext = GameContext.SongContext;
        }
    }
}
