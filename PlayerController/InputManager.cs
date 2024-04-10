using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    public PlayerActions playerActions;
    public AnimatorManager animatorManager;
    public PlayerMovement playerMovement;
    public CameraPositionManager cameraPositionManager;
    GameManager gameManager;

    public float moveAmount;
    private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    private Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;

    [Header("Input button flags")]
    public bool jumpInput;
    public bool bInput;
    public bool interactInput;
    public bool nextInput;
    public bool previousInput;
    public bool buyInput;
    public bool changeRifleInput;
    public bool shootInput;
    public bool scopeInput;
    public bool reloadInput;
    public bool carInteractInput;
    public bool pauseInput;

    [Header("Joysticks")]
    public FixedJoystick movementJoyStick;
    public FixedJoystick cameraJoyStick;


    void Awake() {
        cameraPositionManager = FindObjectOfType<CameraPositionManager>();
        animatorManager = FindObjectOfType<AnimatorManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnEnable() {
        if(playerActions == null) {
            playerActions = new PlayerActions();
            playerActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerActions.PlayerMovement.CameraMovement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerActions.PlayerAction.B.performed += i => bInput = true;
            playerActions.PlayerAction.B.canceled += i => bInput = false;
            playerActions.PlayerAction.Jump.performed += i => jumpInput = true;
            playerActions.PlayerAction.Interact.performed += i => interactInput = true;
            playerActions.PlayerAction.Next.performed += i => nextInput = true;
            playerActions.PlayerAction.Previous.performed += i => previousInput = true;
            playerActions.PlayerAction.Buy.performed += i => buyInput = true;
            playerActions.PlayerAction.ChangeRifle.performed += i => changeRifleInput = true;
            playerActions.PlayerAction.Shoot.performed += i => shootInput = true;
            playerActions.PlayerAction.Shoot.canceled += i => shootInput = false;
            playerActions.PlayerAction.Scope.performed += i => scopeInput = true;
            playerActions.PlayerAction.Scope.canceled += i => scopeInput = false;
            playerActions.PlayerAction.Reloading.performed += i => reloadInput = true;
            playerActions.PlayerAction.Reloading.canceled += i => reloadInput = false;
            playerActions.PlayerAction.Interact.performed += i => carInteractInput = true;
            playerActions.PlayerAction.Interact.canceled += i => carInteractInput = false;
            playerActions.PlayerAction.Pause.performed += i => pauseInput = true;
        }

        playerActions.Enable();
    }

    void OnDisable() {
        playerActions.Disable();
    }

    public void HandleAllInputs() {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleInteractInput();
        HandleNextInput();
        HandlePreviousInput();
        HandleBuyInput();
        HandleChangeRifleInput();
        HandlePauseInput();
    }

    private void HandleMovementInput() {
        if(gameManager.useMobileInputs == true) {
            verticalInput = movementJoyStick.Vertical;
            horizontalInput = movementJoyStick.Horizontal;

            cameraInputX = cameraJoyStick.Horizontal;
            cameraInputY = cameraJoyStick.Vertical;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animatorManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);
        }
        else {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            cameraInputX = cameraInput.x;
            cameraInputY = cameraInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animatorManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);
        }
        
    }

    private void HandleSprintingInput() {
        if(gameManager.useMobileInputs && CrossPlatformInputManager.GetButton("Sprint")) {
            playerMovement.isSprinting = true;
        }
        else if(bInput && moveAmount > 0.5f) {
            playerMovement.isSprinting = true;
        }
        else {
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpingInput() {
        if(gameManager.useMobileInputs && CrossPlatformInputManager.GetButton("Jump")) {
            jumpInput = false;
            playerMovement.isJumping = true;
            playerMovement.HandleJumping();
        }
        if(jumpInput) {
            jumpInput = false;
            playerMovement.isJumping = true;
            playerMovement.HandleJumping();
        }
    }

    private void HandleInteractInput() {
        if(interactInput) {
            interactInput = false;
        }
    }

    private void HandleNextInput() {
        if(nextInput) {
            cameraPositionManager.NextPosition();
            nextInput = false;
        }
    }

    private void HandlePreviousInput() {
        if(previousInput) {
            cameraPositionManager.PreviousPosition();
            previousInput = false;
        }
    }

    private void HandleBuyInput() {
        if(buyInput) {
            cameraPositionManager.BuyItem();
            if(gameManager.Mission1 == true && gameManager.Mission2 == false && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false) {
                gameManager.Mission2 = true;
            }
            buyInput = false;
        }
    }

    private void HandleChangeRifleInput() {
        if(changeRifleInput) {
            changeRifleInput = false;
        }
    }

    private void HandlePauseInput() {
        if(pauseInput) {
            pauseInput = false;
        }
    }
}
