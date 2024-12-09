using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text hpText;

    [SerializeField] private Slider specialBar;
    [SerializeField] private TMP_Text specialText;

    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button continueButton;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        settingButton.onClick.AddListener(OnSettingBtn);
        continueButton.onClick.AddListener(OnContinueBtn);
        exitButton.onClick.AddListener(OnExitBtn);

        //
        float Hp = GameplayManager.Instance.player.GetComponent<Character>().CurrentHP;
        UpdateHpHUD(Hp, Hp);
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
    public void UpdateHpHUD(float currentHp, float maxHp)
    {
        if (currentHp < 0) currentHp = 0;
        hpBar.value = currentHp / maxHp;
        hpText.text = currentHp.ToString() + " / " + maxHp.ToString();
    }

    public void UpdateSpecialHUD(float currentSpecial, float maxSpecial)
    {
        specialBar.value = currentSpecial / maxSpecial;
        specialText.text = currentSpecial.ToString() + " / " + maxSpecial.ToString();
    }

    public void OnExitBtn()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnContinueBtn()
    {
        settingPanel.SetActive(false);
        Cursor.visible = false;
    }

    public void OnSettingBtn()
    {
        settingPanel.SetActive(true);
        Cursor.visible = true;
    }
}
