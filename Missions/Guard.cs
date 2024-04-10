using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
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

    Animator animator;

    [Header("Guards AI")]
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
    GameManager gameManager; 
    public GameObject bloodEffect; 

    void Start() {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        currentMovingSpeed = movingSpeed;
        presentHealth = characterHealth;
        // playerBody = GameObject.Find("Player");
    }

    void Update() {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if(!playerInVisionRadius && !playerInShootingRadius) {
            Idle();
        }

        if(playerInVisionRadius && !playerInShootingRadius) {
            ChasePlayer();
        }

        if(playerInVisionRadius && playerInShootingRadius) {
            ShootPlayer();
        }
    }

    public void Idle() {
        currentMovingSpeed = 0f;
        animator.SetBool("Run", false);
        animator.SetBool("Shoot", false);
    }

    public void ChasePlayer() {
        transform.position += transform.forward * currentMovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", true);
        animator.SetBool("Shoot", false); 

        currentMovingSpeed = runningSpeed;
    }

    void ShootPlayer() {
        currentMovingSpeed = 0;
        transform.LookAt(playerBody.transform);

        animator.SetBool("Run", false);
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
                Debug.Log("Guards shooting ..");
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
        currentMovingSpeed = 0f;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Object.Destroy(gameObject, 7f);
    }
}
