using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int playerMoney;
    public float[] position;
    public bool pistolPrefab;
    public bool AKMPrefab;
    public bool M416Prefab;

    public bool Mission1;
    public bool Mission2;
    public bool Mission3;
    public bool Mission4;
    public bool Mission5;

    public PlayerData(GameManager gameManager) {
        playerMoney = gameManager.playerMoney;

        position = new float[3];
        position[0] = gameManager.transform.position.x;
        position[1] = gameManager.transform.position.y;
        position[2] = gameManager.transform.position.z;

        pistolPrefab = gameManager.pistolPrefab;
        AKMPrefab = gameManager.AKMPrefab;
        M416Prefab = gameManager.M416Prefab;

        Mission1 = gameManager.Mission1;
        Mission2 = gameManager.Mission2;
        Mission3 = gameManager.Mission3;
        Mission4 = gameManager.Mission4;
        Mission5 = gameManager.Mission5;

        
    }
}
