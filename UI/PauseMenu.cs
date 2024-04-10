using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;
    public GameObject PauseUI;
    public GameObject OtherUI;

    private bool isPaused = false;

    void Start() {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if(gameManager.useMobileInputs == true) {
            if(CrossPlatformInputManager.GetButtonDown("Pause")) {
                if(isPaused) {
                    Resume();
                }
                else {
                    Pause();
                }
            }
        }
        else {
            if(inputManager.pauseInput) {
                if(isPaused) {
                    Resume();
                }
                else {
                    Pause();
                }
            }
        }
    }

    public void Resume() {
        PauseUI.SetActive(false);
        OtherUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause() {
        PauseUI.SetActive(true);
        OtherUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame() {
        SceneManager.LoadScene("ViceCity");
        Time.timeScale = 1f;
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
    }
}
