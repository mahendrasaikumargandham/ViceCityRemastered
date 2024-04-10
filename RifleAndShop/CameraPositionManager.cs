using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CameraPositionManager : MonoBehaviour
{
    public Transform[] cameraPositions;
    public int currentPositionIndex;

    private GameManager gameManager;
    private ColliderAction colliderAction;
    public Text priceText;
    public GameObject shopControls;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        colliderAction = FindObjectOfType<ColliderAction>();

        transform.position = cameraPositions[currentPositionIndex].position;
        transform.rotation = cameraPositions[currentPositionIndex].rotation;
    }

    void Update() {
        if(gameManager.useMobileInputs == true && colliderAction.inShop == true) { 
            if(CrossPlatformInputManager.GetButtonDown("N")) {
                NextPosition();
            }
            if(CrossPlatformInputManager.GetButtonDown("P")) {
                PreviousPosition();
            }
            if(CrossPlatformInputManager.GetButtonDown("B")) {
                BuyItem();
                if(gameManager.Mission1 == true && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false) {
                    gameManager.Mission2 = true;
                }
            }
        }
        if(currentPositionIndex == 0 && colliderAction.playerTriggered == true) {
            int itemPrice = 50;
            priceText.text = "Price: $" + itemPrice;
        }
        
        if(currentPositionIndex == 1 && colliderAction.playerTriggered == true) {
            int itemPrice = 70;
            priceText.text = "Price: $" + itemPrice;
        }

        if(currentPositionIndex == 2 && colliderAction.playerTriggered == true) {
            int itemPrice = 30;
            priceText.text = "Price: $" + itemPrice;
        }
    }

    void MoveCamera() {
        transform.position = cameraPositions[currentPositionIndex].position;
        transform.rotation = cameraPositions[currentPositionIndex].rotation;
    }

    public void NextPosition() {
        currentPositionIndex = (currentPositionIndex + 1) % cameraPositions.Length;
        MoveCamera();
    }

    public void PreviousPosition() {
        currentPositionIndex = (currentPositionIndex - 1 + cameraPositions.Length) % cameraPositions.Length;
        MoveCamera();
    }

    public void BuyItem() {
        if(currentPositionIndex == 0 && colliderAction.playerTriggered == true && gameManager.AKMPrefab == false) {
            int itemPrice = 50;
            if(gameManager.playerMoney >= itemPrice) {
                gameManager.playerMoney -= itemPrice;
                gameManager.AKMPrefab = true;
            }
        }
        else if(currentPositionIndex == 1 && colliderAction.playerTriggered == true && gameManager.M416Prefab == false) {
            int itemPrice = 70;
            if(gameManager.playerMoney >= itemPrice) {
                gameManager.playerMoney -= itemPrice;
                gameManager.M416Prefab = true;
            }
        }
        else if(currentPositionIndex == 2 && colliderAction.playerTriggered == true && gameManager.pistolPrefab == false) {
            int itemPrice = 30;
            if(gameManager.playerMoney >= itemPrice) {
                gameManager.playerMoney -= itemPrice;
                gameManager.pistolPrefab = true;
            }
        }
    }
}
