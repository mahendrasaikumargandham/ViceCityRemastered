using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigatorScript : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float turningSpeed;
    public float stopSpeed;

    [Header("NPC Health Settings")]
    private float characterHealth = 10f;
    public float presentHealth;

    [Header("Destination Variables")]
    public Vector3 destination;
    public bool destinationReached;
    public Animator animator;
    private float waypointTimeout = 30f;
    public float currentTimeout = 0f;

    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        presentHealth = characterHealth;
    }

    void Update() {
        Walk();
        if(!destinationReached) {
            currentTimeout += Time.deltaTime;
            if(currentTimeout >= waypointTimeout) {
                KillCharacter();
            }
        }
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
            }
            else {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination) {
        this.destination = destination;
        destinationReached = false;
        currentTimeout = 0f;
    }

    public void CharacterHitDamage(float takeDamage) {
        presentHealth -= takeDamage;
        if(presentHealth <= 0) {
            animator.SetBool("Die", true);
            CharacterDie();
            gameManager.currentKills += 1;
            gameManager.playerMoney += 10;
        }
    }

    private void CharacterDie() {
        movingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 7f);
    }

    private void KillCharacter() {
        presentHealth = 0;
        CharacterDie();
    }
}
