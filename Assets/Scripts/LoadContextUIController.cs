using Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadContextUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] slotObjects = new GameObject[Constants.SAVING_SLOTS_NUMBER];
    [SerializeField] private Text[] dates = new Text[Constants.SAVING_SLOTS_NUMBER];
    [SerializeField] private Text[] names = new Text[Constants.SAVING_SLOTS_NUMBER];
    [SerializeField] private Button playButton;

    private bool contentLoaded = false;
    private int choosedSlot = -1;
    private float scaleRate = 1.2f;

    void Update()
    {
        if (GameManager.State.CurrentMenuContext == MenuContext.LoadContext)
        {
            if (!contentLoaded)
            {
                var availability = DataManager.GetSlotsAvailability();
                for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
                {
                    bool exist = availability.Exists(t => t.id == i);
                    slotObjects[i].GetComponentInChildren<Button>().interactable = exist;
                    names[i].text = exist ? availability.Where(t => t.id == i).First().name : "Brak nazwy";
                    dates[i].text = exist ? availability.Where(t => t.id == i).First().date : "Brak daty";
                    SetColor(exist ? Constants.WHITE : Constants.WHITE_TRANSPARENT, i);
                }
                foreach (var slot in slotObjects)
                    slot.transform.localScale = Vector3.one;
                contentLoaded = true;
            }

            playButton.interactable = choosedSlot >= 0 && choosedSlot < Constants.SAVING_SLOTS_NUMBER;
        }        
    }

    public void OnSlotButtonClick(int id)
    {
        choosedSlot = choosedSlot != id ? id : - 1;
        foreach (var slot in slotObjects)
            slot.transform.localScale = Vector3.one;
        if (choosedSlot >= 0) slotObjects[choosedSlot].transform.localScale *= scaleRate;
    }

    public void OnLoadGameButtonClick()
    {
        if (DataManager.LoadData(choosedSlot))
        {
            choosedSlot = -1;
            contentLoaded = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void OnBackToMenuButtonClick()
    {
        GameManager.State.CurrentMenuContext = MenuContext.MainContext;
        choosedSlot = -1;
        contentLoaded = false;
    }

    private void SetColor(Color col, int i)
    {
        names[i].color = col;
        dates[i].color = col;
    }
}
