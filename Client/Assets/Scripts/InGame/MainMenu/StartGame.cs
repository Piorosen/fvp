using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartGame : MonoBehaviour {

    public InputField input;

    public void Game()
    {
        PlayerPrefs.SetString("PlayerName", input.text);
        SceneManager.LoadScene("InGame", LoadSceneMode.Single);
    }
}
