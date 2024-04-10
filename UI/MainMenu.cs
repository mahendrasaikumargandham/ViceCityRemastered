using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool continueGame = false;
    public bool startGame = false;
    // public GameObject mainMenuUI;

    public static MainMenu instance;

    private void Awake() {
        Cursor.lockState = CursorLockMode.None;
        instance = this;
    }

    public void OnContinueButton() {
        Debug.Log("Continue");
        continueGame = true;
        SceneManager.LoadScene("ViceCity");
    }

    public void OnStartGame() {
        Debug.Log("Start");
        startGame = true;
        SceneManager.LoadScene("ViceCity");
    }

    public void QuitGame() {
        Debug.Log("Quitting the Game");
        Application.Quit();
    }
}
