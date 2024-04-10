using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    GameManager gameManager;
    public Transform playerTransform;
    public Transform camPivot;
    private Vector3 camFollowVelocity = Vector3.zero;

    [Header("Camera movement and rotation")]
    public float camFollowSpeed = 0.1f;
    public float camLookSpeed = 0.1f;
    public float camPivotSpeed = 0.1f;
    public float lookAngle = 90f;
    public float pivotAngle;

    public float minimumPivotAngle = -30f;
    public float maximumPivotAngle = 30f;

    [Header("Scoped Settings")]
    public float scopedFOV = 20f;
    public float defaultFOV = 60f;
    bool isScoped = false;
    PlayerMovement playerMovement;
    public Camera camera;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager = FindObjectOfType<InputManager>();
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void HandleAllCameraMovement() {
        FollowTarget();
        RotateCamera();
        HandleScopedFOV();
    }

    void FollowTarget() {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref camFollowVelocity, camFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera() {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * camLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * camPivotSpeed);

        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        camPivot.localRotation = targetRotation;

        if(gameManager.useMobileInputs == true && isScoped == true) {
            camLookSpeed = .3f;
            camPivotSpeed = .3f;
            minimumPivotAngle = -0.5f;
            maximumPivotAngle = 6f;
            playerTransform.rotation = Quaternion.Euler(pivotAngle, lookAngle, 0);
        }
        else {
            camLookSpeed = 3f;
            camPivotSpeed = 1f;
            minimumPivotAngle = -30f;
            maximumPivotAngle = 30f;
        }

        if(playerMovement.isMoving == false && playerMovement.isSprinting == false) {
            playerTransform.rotation = Quaternion.Euler(0, lookAngle, 0);
        }
    }

    private void HandleScopedFOV() {
        if(gameManager.useMobileInputs == true) {
            if(CrossPlatformInputManager.GetButton("Fire2")) {
                camera.fieldOfView = scopedFOV;
                isScoped = true;
            }
            else {
                camera.fieldOfView = defaultFOV;
                isScoped = false; 
            }
        }
        else {
            if(inputManager.scopeInput) {
                camera.fieldOfView = scopedFOV;
                isScoped = true;
            }
            else {
                camera.fieldOfView = defaultFOV;
                isScoped = false; 
            }
        }
    }
}

// using UnityEngine;

// public class CameraManager : MonoBehaviour
// {
//     InputManager inputManager;
//     public Transform playerTransform;
//     public Transform camPivot;
//     private Vector3 camFollowVelocity = Vector3.zero;

//     [Header("Camera movement and rotation")]
//     public float camFollowSpeed = 0.1f;
//     public float camLookSpeed = 0.05f;
//     public float camPivotSpeed = 0.05f;
//     public float lookAngle = 90f;
//     public float pivotAngle;

//     public float minimumPivotAngle = -30f;
//     public float maximumPivotAngle = 30f;

//     void Awake()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         inputManager = FindObjectOfType<InputManager>();
//         playerTransform = FindObjectOfType<PlayerManager>().transform;
//     }

//     public void HandleAllCameraMovement()
//     {
//         RotateCamera();
//         FollowTarget();
//     }

//     void FollowTarget()
//     {
//         // Calculate target position for the camera (behind the player)
//         Vector3 targetPosition = playerTransform.position - playerTransform.forward * 3f + playerTransform.up * 1f;

//         // Smoothly move the camera towards the target position
//         transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camFollowVelocity, camFollowSpeed);

//         // Make the camera always look at the player's position
//         transform.LookAt(playerTransform.position);
//     }

//     private void RotateCamera()
//     {
//         // Rotate the camera pivot based on mouse input
//         pivotAngle -= inputManager.cameraInputY * camPivotSpeed;
//         pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);
//         camPivot.localRotation = Quaternion.Euler(pivotAngle, 0f, 0f);

//         // Rotate the player horizontally based on mouse input
//         float horizontalRotation = inputManager.cameraInputX * camLookSpeed;
//         playerTransform.Rotate(Vector3.up * horizontalRotation);
//     }
// }

//Camera Controller;

// using UnityEngine;

// public class CameraManager : MonoBehaviour
// {
//     InputManager inputManager;
//     public Transform playerTransform;
//     public Transform camPivot;
//     private Vector3 camFollowVelocity = Vector3.zero;

//     [Header("Camera movement and rotation")]
//     public float camFollowSpeed = 0.1f;
//     public float camLookSpeed = 0.1f;
//     public float camPivotSpeed = 0.1f;
//     public float lookAngle = 90f;
//     public float pivotAngle;
//     public float horizontalRotationSpeed = 2f;

//     public float minimumPivotAngle = -30f;
//     public float maximumPivotAngle = 30f;

//     void Awake()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         inputManager = FindObjectOfType<InputManager>();
//         playerTransform = FindObjectOfType<PlayerManager>().transform;
//     }

//     public void HandleAllCameraMovement()
//     {
//         RotateCamera();
//         FollowTarget();
//     }

//     void FollowTarget()
//     {
//         // Calculate target position for the camera (behind the player)
//         Vector3 targetPosition = playerTransform.position - playerTransform.forward * 0.2f + playerTransform.up * 0.005f; // Adjusted height offset

//         // Smoothly move the camera towards the target position
//         transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref camFollowVelocity, camFollowSpeed);

//         // Make the camera always look at the player's position
//         transform.LookAt(playerTransform.position);
//     }

//     private void RotateCamera()
//     {
//         // Rotate the camera pivot based on mouse input
//         pivotAngle -= inputManager.cameraInputY * camPivotSpeed;
//         pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);
//         camPivot.localRotation = Quaternion.Euler(pivotAngle, 0f, 0f);

//         // Rotate the player horizontally based on mouse input
//         float horizontalRotation = inputManager.cameraInputX * camLookSpeed * horizontalRotationSpeed;
//         playerTransform.Rotate(Vector3.up * horizontalRotation);
//     }
// }

