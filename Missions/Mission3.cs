using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission3 : MonoBehaviour
{
    public Animator characterAnimator;
    public Animator cameraAnimator;
    public GameObject playerCharacter;
    public Text uiText;
    public string[] textArray;
    private float timer = 0f;
    private int currentIndex = 0;
    bool isCollided = false;
    public GameObject mainCamera;
    public GameObject missionCamera;

    public float targetTime = 4f;

    private void Start() {
        if(textArray.Length > 0) {
            uiText.text = textArray[0];
        }
    }

    private void Update() {
        if(!isCollided) {
            uiText.gameObject.SetActive(false);
        }
        else {
            uiText.gameObject.SetActive(true);
        }

        if(isCollided) {
            if(playerCharacter != null) {
                PlayerMovement playerMovement = playerCharacter.GetComponent<PlayerMovement>();
                if(playerMovement != null) {
                    playerMovement.walkingSpeed = 0f;
                    playerMovement.runningSpeed = 0f;
                    playerMovement.sprintingSpeed = 0f;
                }
            }
            mainCamera.SetActive(false);
            missionCamera.SetActive(true);
            characterAnimator.SetBool("StartTalking", true);
            cameraAnimator.SetBool("StartMoving", true);

            timer += Time.deltaTime;
            if(timer >= 10f) {
                timer = 0f;
                currentIndex = (currentIndex + 1) % textArray.Length;
                uiText.text = textArray[currentIndex];
            }
            targetTime -= Time.deltaTime;
        }
        if(targetTime <= 0 && isCollided == true) {
            isCollided = false;
            uiText.text = "";
            Destroy(gameObject);

            if(playerCharacter != null) {
                PlayerMovement playerMovement = playerCharacter.GetComponent<PlayerMovement>();
                if(playerMovement != null) {
                    playerMovement.walkingSpeed = 2f;
                    playerMovement.runningSpeed = 5f;
                    playerMovement.sprintingSpeed = 7f;
                }
            }

            mainCamera.SetActive(true);
            missionCamera.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            isCollided = true;
        }
    }
}
