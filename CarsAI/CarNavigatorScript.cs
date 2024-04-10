using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNavigatorScript : MonoBehaviour
{
    [Header("Car Info")]
    public float movingSpeed;
    public float turningSpeed;
    public float stopSpeed;
    public Transform[] wheels;


    [Header("Destination Variables")]
    public Vector3 destination;
    public bool destinationReached;


    void Start() {
        // gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        Walk();
    }

    public void Walk() {
        if(transform.position != destination) {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopSpeed) {
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
                RotateWheels();          
            }
            else {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination) {
        this.destination = destination;
        destinationReached = false;
        // currentTimeout = 0f;
    }

    private void RotateWheels() {
        foreach(Transform wheel in wheels) {
            wheel.Rotate(Vector3.right, movingSpeed * Time.deltaTime * 200f);
        }
    }
}
