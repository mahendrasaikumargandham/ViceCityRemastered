using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character Health Settings")]
    private float characterHealth = 100f;
    public float presentHealth;
    public Text characterHealthText;

    [Header("Script reference")]
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    [Header("Player Movement")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody playerRigidBody;

    [Header("Movement flags")]
    public bool isMoving;
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement values")]
    public float walkingSpeed = 1.7f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 12f;

    [Header("Falling and Landing")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Jump variables")]
    public float jumpHeight = 4f;
    public float gravityIntensity = -15f;


    void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        presentHealth = characterHealth;  
        characterHealthText.text = "❤️" + characterHealth; 
    }

    public void HandleAllMovement() {
        HandleFallingAndLanding();
        if(playerManager.isInteracting) 
            return;
        HandleMovement();
        HandleRotation();   
    }

    private void HandleMovement() {
        if(isJumping) 
            return;

        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + camObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(isSprinting) {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else {
            if(inputManager.moveAmount >= 0.5f) {
                moveDirection = moveDirection * runningSpeed;
                isMoving = true;
            }
            else if(inputManager.moveAmount >= 0.5f) {
                moveDirection = moveDirection * walkingSpeed;
                isMoving = false;
            }
        }

        Vector3 movementVelocity = moveDirection;

        playerRigidBody.velocity = movementVelocity;
    }

    private void HandleRotation() {
        if(isJumping) 
            return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = camObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + camObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero) {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    void HandleFallingAndLanding() {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 targetPosition;
        raycastOrigin.y = raycastOrigin.y + raycastHeightOffset;
        targetPosition = transform.position;

        if(!isGrounded && !isJumping) {
            if(!playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidBody.AddForce(transform.forward * leapingVelocity);
            playerRigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if(Physics.SphereCast(raycastOrigin, 0.3f, -Vector3.up, out hit, groundLayer)) {
            if(!isGrounded && !playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Landing", true);
            }

            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }

        if(isGrounded && !isJumping) {
            if(playerManager.isInteracting || inputManager.moveAmount > 0) {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping() {
        if(isGrounded) {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);

            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpVelocity;
            playerRigidBody.velocity = playerVelocity;

            isJumping = false;
        }
    }

    public void SetIsJumping(bool isJumping) {
        this.isJumping = isJumping;
    }

    public void CharacterHitDamage(float takeDamage) {
        presentHealth -= takeDamage;
        characterHealthText.text = "❤️" + presentHealth;
        if(presentHealth <= 0) {
            CharacterDie();
        }
    }

    private void CharacterDie() {
        Object.Destroy(gameObject);
    }
}
