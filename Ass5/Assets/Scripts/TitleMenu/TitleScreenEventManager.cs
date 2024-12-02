using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenEventManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject chooseMode;
    public GameObject chooseLevel;
    public GameObject setting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBackToChooseMode()
    {
        chooseMode.SetActive(true);
        chooseLevel.SetActive(false);
    }
    public void OnBackToMenu()
    {
        menu.SetActive(true);
        chooseMode.SetActive(false);
        chooseLevel.SetActive(false);
        setting.SetActive(false);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnStartGame()
    {
        chooseMode.SetActive(true);
        menu.SetActive(false);
    }
    public void OnSetting()
    {
        setting.SetActive(true);
        menu.SetActive(false);
    }
    public void OnContinue()
    {

    }
    public void OnSetVolume()
    {
    }
    public void OnSetSFX()
    {

    }
    public void OnPVP()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }
    public void OnPVE()
    {
        chooseLevel.SetActive(true);
        chooseMode.SetActive(false);
    }
    public void OnLevel1()
    {
    }
    public void OnLevel2()
    {
    }
    public void OnLevel3()
    {
    }

}
