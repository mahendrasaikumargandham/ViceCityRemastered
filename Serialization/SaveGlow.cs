using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGlow : MonoBehaviour
{
    GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            gameManager.SavePlayer();
        }
    }
}
