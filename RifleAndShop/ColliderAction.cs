using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ColliderAction : MonoBehaviour
{
    GameManager gameManager;
    InputManager inputManager;
    public GameObject mainCamera;
    public GameObject secondaryCamera;


    public Text notificationText;
    public GameObject priceText;

    public bool playerTriggered = false;
    public bool inShop;

    void Start() {
        secondaryCamera.SetActive(false);
        notificationText.gameObject.SetActive(false);
        priceText.SetActive(false);

        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update() {
        if(playerTriggered) {
            if(gameManager.useMobileInputs == true) {
                if(CrossPlatformInputManager.GetButtonDown("E") && inShop == false) {
                    mainCamera.SetActive(false);
                    secondaryCamera.SetActive(true);
                    priceText.SetActive(true);  
                    notificationText.gameObject.SetActive(false);
                    inShop = true;
                }
                else if(CrossPlatformInputManager.GetButtonDown("E") && inShop == true) {
                    mainCamera.SetActive(true);
                    secondaryCamera.SetActive(false);
                    priceText.SetActive(false);  
                    notificationText.gameObject.SetActive(false);
                    inShop = false;
                }
            }
            else {
                if(inputManager.interactInput && inShop == false) {
                    mainCamera.SetActive(false);
                    secondaryCamera.SetActive(true);
                    priceText.SetActive(true);  
                    notificationText.gameObject.SetActive(false);
                    inShop = true;
                }
                else if(inputManager.interactInput && inShop == true) {
                    mainCamera.SetActive(true);
                    secondaryCamera.SetActive(false);
                    priceText.SetActive(false);  
                    notificationText.gameObject.SetActive(false);
                    inShop = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            notificationText.text = "Press E to continue";
            notificationText.gameObject.SetActive(true);
            playerTriggered = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            notificationText.gameObject.SetActive(false);
            playerTriggered = false;
        }
    }
}
