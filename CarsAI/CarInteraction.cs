using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarInteraction : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;

    public GameObject car1;
    public GameObject player;

    GameObject currentCar;
    public GameObject playerCamera;
    public Transform exitPosition;

    public bool playerInCar;
    public int carSeatRange = 3;

    public KeyCode carInteract = KeyCode.E;

    

    void Start() {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if(playerInCar == true) {
            playerCamera.transform.position = exitPosition.transform.position;
            player.transform.position = exitPosition.transform.position;
            inputManager.jumpInput = false;
        }
        
        

        if(gameManager.useMobileInputs) {
            if(CrossPlatformInputManager.GetButtonDown("E")) {
                if(currentCar == null) {
                    TryEnterCar(car1);
                }
                else {
                    ExitCar();
                }
            }
        }
        else {
            if(Input.GetKeyDown(carInteract)) {
                if(currentCar == null) {
                    TryEnterCar(car1);
                }
                else {
                    ExitCar();
                }
            }
        }
        
    }

    void TryEnterCar(GameObject car) {
        if(car != null && Vector3.Distance(player.transform.position, car.transform.position) < carSeatRange) {
            currentCar = car;
            exitPosition = currentCar.transform.Find("CarExitPoint");
            car.GetComponent<PrometeoCarController>().enabled = true;
            car.transform.Find("Camera").gameObject.SetActive(true);
            player.SetActive(false);
            playerCamera.gameObject.SetActive(false);
            playerInCar = true;
        }
    }

    void ExitCar() {
            currentCar.GetComponent<PrometeoCarController>().enabled = false;
            currentCar.transform.Find("Camera").gameObject.SetActive(false);

            exitPosition = currentCar.transform.Find("CarExitPoint");
            if(exitPosition != null) {
                player.transform.position = exitPosition.position;
            }
            else {
                Debug.Log("car exit not found in the car");
                player.transform.position = exitPosition.transform.position;
            }

            player.SetActive(true);
            playerCamera.gameObject.SetActive(true);
            currentCar = null;
            playerInCar = false;
        }
}
