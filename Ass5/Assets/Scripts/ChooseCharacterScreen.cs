using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterScreen : MonoBehaviour
{
    public void OnChooseCharacter(string characterType)
    {
        GameManager.Instance.CharacterType = characterType;
        UnityEngine.SceneManagement.SceneManager.LoadScene("PVEGameplay");
    }
}
