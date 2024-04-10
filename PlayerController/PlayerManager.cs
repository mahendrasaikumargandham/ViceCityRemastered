using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    CameraManager cameraManager;
    Animator animator;

    public bool isInteracting;

    void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        inputManager.HandleAllInputs();
        cameraManager.HandleAllCameraMovement();
    }

    void FixedUpdate() {
        playerMovement.HandleAllMovement();
    }

    void LateUpdate() {
        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
