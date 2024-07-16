using Scripts;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIController : MonoBehaviour
{
    [SerializeField] private GameObject SaveWindowBox;
    [SerializeField] private GameObject[] slotObjects = new GameObject[Constants.SAVING_SLOTS_NUMBER];
    [SerializeField] private Text[] dates = new Text[Constants.SAVING_SLOTS_NUMBER];
    [SerializeField] private Text[] names = new Text[Constants.SAVING_SLOTS_NUMBER];

    [SerializeField] private InputField inputName;
    [SerializeField] private Button saveButton;

    private bool contentLoaded = false;
    private int choosedSlot = -1;
    private float scaleRate = 1.2f;

    void Update()
    {
        SaveWindowBox.SetActive(GameManager.SaveWindowShown);
        inputName.transform.localScale = GameManager.SaveWindowShown ? Vector3.one : Vector3.zero;

        if (GameManager.SaveWindowShown)
        {
            if (!contentLoaded)
            {
                var availability = DataManager.GetSlotsAvailability();
                for (int i = 0; i < Constants.SAVING_SLOTS_NUMBER; i++)
                {
                    bool exist = availability.Exists(t => t.id == i);
                    names[i].text = exist ? availability.Where(t => t.id == i).First().name : "Brak nazwy";
                    dates[i].text = exist ? availability.Where(t => t.id == i).First().date : "Brak daty";
                    slotObjects[i].GetComponentInChildren<Image>().color = exist ? Constants.WHITE : Constants.WHITE_LITTLE_TRANSPARENT;
                }
                foreach (var slot in slotObjects)
                    slot.transform.localScale = Vector3.one;
                contentLoaded = true;
            }

            saveButton.interactable = choosedSlot >= 0 && choosedSlot < Constants.SAVING_SLOTS_NUMBER && inputName.text != "";
        }
    }

    public void OnSlotButtonClick(int id)
    {
        choosedSlot = choosedSlot != id ? id : -1;
        foreach (var slot in slotObjects)
            slot.transform.localScale = Vector3.one;
        if (choosedSlot >= 0) slotObjects[choosedSlot].transform.localScale *= scaleRate;
    }

    public void OnSaveButtonClick()
    {
        DataManager.SaveData(choosedSlot, inputName.text);
        CloseWindow();
    }

    public void OnCloseButtonClick()
    {
        CloseWindow();
    }

    private void CloseWindow()
    {
        GameManager.SaveWindowShown = false;
        choosedSlot = -1;
        contentLoaded = false;
        inputName.text = "";
    }
}
