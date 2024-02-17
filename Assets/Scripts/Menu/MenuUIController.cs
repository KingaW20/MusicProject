using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] contexts = new GameObject[3];

    public enum MenuContext
    {
        MainContext,
        PlayContext,
        LoadContext
    }

    void Start()
    {
        SwitchContext(MenuContext.MainContext);
    }

    public void OnPlayButtonClick()
    {
        //TODO: jeœli bêdzie mo¿liwoœæ wyboru kategorii, które Ci pasuj¹
        //SwitchContext(MenuContext.PlayContext);

        //SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        SceneManager.LoadScene("Game");
    }

    public void OnLoadButtonClick()
    {
        SwitchContext(MenuContext.LoadContext);
    }

    private void SwitchContext(MenuContext context)
    {
        foreach (var contextObject in contexts)
            contextObject.SetActive(false);
        contexts[(int)context].SetActive(true);
    }
}
