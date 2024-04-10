using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    [Header("Player Rifle things")]
    public GameObject pistolGameObjectPrefab;
    public GameObject AKMGameObjectPrefab;
    public GameObject M416GameObjectPrefab;

    public bool pistolPrefab;
    public bool AKMPrefab;
    public bool M416Prefab;

    public bool rifle1Active;
    public bool rifle2Active;
    public bool rifle3Active;

    [Header("Player Money and Kills")]
    public int playerMoney = 150;
    public int currentKills = 0;

    public Text playerMoneyText;
    public Text playerHealthText;

    public GameObject rifleImage;
    public GameObject akmImage;
    public GameObject m416Image;
    public GameObject fistImage;

    [Header("UI and Animations")]
    AnimatorManager animatorManager;
    InputManager inputManager;

    [Header("Missions")]
    public bool Mission1;
    public GameObject mission1Area;

    public bool Mission2;
    public bool Mission3;
    public GameObject Mission3Area;

    public bool Mission4;
    public bool Mission5;
    public GameObject mission4and5Area;

    public Transform playerGameObject;
    public Transform cameraGameObject;

    public bool useMobileInputs = false;

    [Header("Car UI")]
    public GameObject shootingUI;
    public GameObject carUI;
    public GameObject shopControls;
    public GameObject playerControls;

    private ColliderAction colliderAction;
    CarInteraction carInteraction;

    void Start() {
        if(MainMenu.instance.continueGame == true) {
            LoadPlayer();
            cameraGameObject.position = playerGameObject.position;
        }
        animatorManager = FindObjectOfType<AnimatorManager>();
        inputManager = FindObjectOfType<InputManager>();
        colliderAction = FindObjectOfType<ColliderAction>();
        carInteraction = FindObjectOfType<CarInteraction>();
        playerMoneyText.text = "$000000" + playerMoney;
    }

    void Update() {
        if(carInteraction.playerInCar == true && colliderAction.inShop == false && !rifle1Active && !rifle2Active && !rifle3Active && useMobileInputs) {
            playerControls.SetActive(false);
            shopControls.SetActive(false);
            shootingUI.SetActive(false);
            carUI.SetActive(true);
        }
        else if(!carInteraction.playerInCar && colliderAction.inShop && !rifle1Active && !rifle2Active && !rifle3Active && useMobileInputs) {
            playerControls.SetActive(false);
            shopControls.SetActive(true);
            shootingUI.SetActive(false);
            carUI.SetActive(false);
        }
        else if(!carInteraction.playerInCar && !colliderAction.inShop && (rifle1Active || rifle2Active || rifle3Active) && useMobileInputs) {
            playerControls.SetActive(true);
            shopControls.SetActive(false);
            shootingUI.SetActive(true);
            carUI.SetActive(false);
        }
        else {
            playerControls.SetActive(true);
            shopControls.SetActive(false);
            shootingUI.SetActive(false);
            carUI.SetActive(false);
        }
        playerMoneyText.text = "$000000" + playerMoney;
        if(inputManager.changeRifleInput || (useMobileInputs && CrossPlatformInputManager.GetButtonDown("RifleChange"))) {
            if(pistolPrefab == true && !rifle1Active) {
                SetPistol();
                rifle1Active = true;
            }
            else if(AKMPrefab == true && !rifle2Active) {
                SetAKM();
                rifle2Active = true;
            }
            else if(M416Prefab == true && !rifle3Active) {
                SetM416(); 
                rifle3Active = true;
            }
            else if(pistolPrefab == true && rifle1Active || AKMPrefab == true && rifle2Active || M416Prefab == true && rifle3Active) {
                rifle1Active = false;
                rifle2Active = false;
                rifle3Active = false;

                animatorManager.animator.SetBool("rifleActive", false);
                animatorManager.animator.SetBool("pistolActive", false);

                pistolGameObjectPrefab.SetActive(false);
                AKMGameObjectPrefab.SetActive(false);
                M416GameObjectPrefab.SetActive(false);
            }
        }

        if(rifle1Active && !rifle2Active && !rifle3Active) {
            rifleImage.SetActive(true);
            akmImage.SetActive(false);
            m416Image.SetActive(false);
            fistImage.SetActive(false);
        }
        else if(rifle1Active && rifle2Active && !rifle3Active) {
            rifleImage.SetActive(false);
            akmImage.SetActive(true);
            m416Image.SetActive(false);
            fistImage.SetActive(false);
        }
        else if(rifle1Active && rifle2Active && rifle3Active) {
            rifleImage.SetActive(false);
            akmImage.SetActive(false);
            m416Image.SetActive(true);
            fistImage.SetActive(false);
        }
        else {
            rifleImage.SetActive(false);
            akmImage.SetActive(false);
            m416Image.SetActive(false);
            fistImage.SetActive(true);
        }

        //Missions
        if(Mission1 == true || Mission2 == true) {
            mission1Area.SetActive(false);
        }

        if(Mission1 == true && Mission2 == true) {
            Mission3Area.SetActive(true);
        }

        if(Mission1 == true && Mission2 == true && Mission3 == true) {
            mission4and5Area.SetActive(true);
        }

        if(Mission1 == true && Mission2 == true && Mission3 == true && Mission4 == true && Mission5 == true) {
            mission4and5Area.SetActive(false);
        }
    }

    void SetPistol() {
        if(!rifle1Active) {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", true);

            pistolGameObjectPrefab.SetActive(true);
            AKMGameObjectPrefab.SetActive(false);
            M416GameObjectPrefab.SetActive(false);

            rifle1Active = true;
        }
        else {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameObjectPrefab.SetActive(false);
            rifle1Active = false;
        }
    }

    void SetAKM() {
        if(!rifle2Active) {
            animatorManager.animator.SetBool("rifleActive", true);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameObjectPrefab.SetActive(false);
            AKMGameObjectPrefab.SetActive(true);
            M416GameObjectPrefab.SetActive(false);

            rifle2Active = true;
        }
        else {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            AKMGameObjectPrefab.SetActive(false);
            rifle2Active = false;
        }
    }

    void SetM416() {
        if(!rifle3Active) {
            animatorManager.animator.SetBool("rifleActive", true);
            animatorManager.animator.SetBool("pistolActive", false);

            pistolGameObjectPrefab.SetActive(false);
            AKMGameObjectPrefab.SetActive(false);
            M416GameObjectPrefab.SetActive(true);

            rifle3Active = true;
        }
        else {
            animatorManager.animator.SetBool("rifleActive", false);
            animatorManager.animator.SetBool("pistolActive", false);

            M416GameObjectPrefab.SetActive(false);
            rifle3Active = false;
        }
    }

    public void SavePlayer() {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer() {
        PlayerData data = SaveSystem.LoadPlayer();
        playerMoney = data.playerMoney;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        pistolPrefab = data.pistolPrefab;
        AKMPrefab = data.AKMPrefab;
        M416Prefab = data.M416Prefab;

        Mission1 = data.Mission1;
        Mission2 = data.Mission2;
        Mission3 = data.Mission3;
        Mission4 = data.Mission4;
        Mission5 = data.Mission5;
    }
}
