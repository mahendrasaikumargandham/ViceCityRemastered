using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameManager gameManager;
    public Animator characterAnimator;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth;
    }

    public void CharacterHitDamage(float damageAmount) {
        currentHealth -= damageAmount;
        if(currentHealth <= 0) {
            Die();
        }
    }
     
    void Die() {
        characterAnimator.SetBool("Die", true);
        if(gameManager.Mission1 == true && gameManager.Mission2 == true && gameManager.Mission3 == false && gameManager.Mission4 == false && gameManager.Mission5 == false) {
            gameManager.Mission3 = true;
        }
        Invoke("DestroyCharacter", 10f);
    }

    void DestroyCharacter() {
        Destroy(gameObject);
    }
}
