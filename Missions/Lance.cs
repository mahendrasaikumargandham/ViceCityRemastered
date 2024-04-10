using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    public float diazHealh = 120f;
    Animator animator;
    GameManager gameManager;
    public float damageAmount = 10f;
    public float shootingCoolDown = 2f;
    private float lastTimeShoot;
    public GameObject bloodEffect;
    public Transform shootingArea;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }
    void Update() {
        if(diazHealh <= 0) {
            if(gameManager.Mission1 && gameManager.Mission2 && gameManager.Mission3) {
                gameManager.Mission4 = true;
                gameManager.playerMoney += 2000;
            }
            Object.Destroy(gameObject, 4f);
            animator.SetBool("Died", true);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        ShootPlayer();
    }

    void ShootPlayer() {
        if(Time.time - lastTimeShoot >= shootingCoolDown) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if(player != null) {
                RaycastHit hit;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                if(Physics.Raycast(shootingArea.position, direction, out hit)) {
                    if(hit.collider.CompareTag("Player")) {
                        PlayerMovement playerHealth = hit.collider.GetComponent<PlayerMovement>();
                        if(playerHealth != null) {
                            playerHealth.CharacterHitDamage(damageAmount);
                            GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                            Destroy(bloodEffectGo, 1f);
                            
                            // animator.SetBool("Shooting", true);
                        }
                        lastTimeShoot = Time.time;
                    }
                }
            }
        }
    }

    public void CharacterHitDamage(float takeDamage) {
        diazHealh -= takeDamage;
    }
}
