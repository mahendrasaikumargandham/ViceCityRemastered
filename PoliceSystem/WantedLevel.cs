using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WantedLevel : MonoBehaviour
{
    GameManager gameManager;

    public bool Level1 = false;
    public bool Level2 = false;
    public bool Level3 = false;
    public bool Level4 = false;
    public bool Level5 = false;

    public Image WantedLevel1;
    public Image WantedLevel2;
    public Image WantedLevel3;
    public Image WantedLevel4;
    public Image WantedLevel5;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if(gameManager.currentKills == 1) {
            SetStarColor(WantedLevel1, true);
            Level1 = true;
        }
        
        if(gameManager.currentKills == 3) {
            SetStarColor(WantedLevel2, true);
            Level2 = true;
        }

        if(gameManager.currentKills == 5) {
            SetStarColor(WantedLevel3, true);
            Level3 = true;
        }
        
        if(gameManager.currentKills == 10) {
            SetStarColor(WantedLevel4, true);
            Level4 = true;
        }

        if(gameManager.currentKills == 20) {
            SetStarColor(WantedLevel5, true);
            Level5 = true;
        }

        SetStarColor(WantedLevel1, Level1);
        SetStarColor(WantedLevel2, Level2);
        SetStarColor(WantedLevel3, Level3);
        SetStarColor(WantedLevel4, Level4);
        SetStarColor(WantedLevel5, Level5);
    }

    private void SetStarColor(Image star, bool isVisible) {
        Color color = star.color;
        color.a = isVisible ? 1f : 0.31f;
        star.color = color;
    }
}
