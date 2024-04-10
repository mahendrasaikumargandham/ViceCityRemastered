using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceOfficer : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    private float currentMovingSpeed;

    public float turningSpeed = 300f;
    public float stopSpeed = 1f;

    [Header("Character Health Settings")]
    private float characterHealth = 100f;
    public float presentHealth;

    [Header("Destination variables")]
    public Vector3 destination;
    public bool destinationReached;
    Animator animator;

    [Header("Police AI")]
    public GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;

    [Header("Character Shooting variables")]
    public float giveDamageOf = 3f;
    public float shootingRange = 100f;
    public GameObject shootingRayCastArea;
    public float timeBtwShoot;
    bool previouslyShoot;
    WantedLevel wantedLevelScript;
    GameManager gameManager; 
    public GameObject bloodEffect; 

    void Start() {
        animator = GetComponent<Animator>();
        wantedLevelScript = FindObjectOfType<WantedLevel>();
        gameManager = FindObjectOfType<GameManager>();
        currentMovingSpeed = movingSpeed;
        presentHealth = characterHealth;
        // playerBody = GameObject.Find("Player");
    }

    void Update() {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if(wantedLevelScript.Level1 == false && wantedLevelScript.Level2 == false && wantedLevelScript.Level3 == false && wantedLevelScript.Level4 == false && wantedLevelScript.Level5 == false) {
            Walk();
        }

        if(playerInVisionRadius && !playerInShootingRadius && (wantedLevelScript.Level1 == true || wantedLevelScript.Level2 == true || wantedLevelScript.Level3 == true || wantedLevelScript.Level4 == true || wantedLevelScript.Level5 == true)) {
            ChasePlayer();
        }

        if(playerInVisionRadius && playerInShootingRadius && (wantedLevelScript.Level1 == true || wantedLevelScript.Level2 == true || wantedLevelScript.Level3 == true || wantedLevelScript.Level4 == true || wantedLevelScript.Level5 == true)) {
            ShootPlayer();
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

                animator.SetBool("Walk", true);
                animator.SetBool("Shoot", false);
                animator.SetBool("Run", false);
            }
            else {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination) {
        this.destination = destination;
        destinationReached = false;
    }

    public void ChasePlayer() {
        transform.position += transform.forward * currentMovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", false); 

        currentMovingSpeed = runningSpeed;
    }

    void ShootPlayer() {
        currentMovingSpeed = 0;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", true);

        if(!previouslyShoot) {
            RaycastHit hit;
            if(Physics.Raycast(shootingRayCastArea.transform.position, shootingRayCastArea.transform.forward, out hit, shootingRange)) {
                PlayerMovement playerMovement = hit.transform.GetComponent<PlayerMovement>();

                if(playerMovement != null) {
                    playerMovement.CharacterHitDamage(giveDamageOf);
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffectGo, 4f);
                }
                Debug.Log("Police shooting ..");
            }
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timeBtwShoot);
        }
    }

    private void ActiveShooting() {
        previouslyShoot = false;
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
        shootingRange = 0f;
        movingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 7f);
    }
}
