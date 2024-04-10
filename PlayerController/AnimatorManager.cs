using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    private int horizontal;
    private int vertical;

    void Awake() {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting) {
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if(horizontalMovement > 0f && horizontalMovement < 0.55f) {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalMovement > 0.55f) {
            snappedHorizontal = 1f;
        }
        else if(horizontalMovement < 0f && horizontalMovement > -0.55f) {
            snappedHorizontal = -0.5f;
        } 
        else if(horizontalMovement < -0.55f) {
            snappedHorizontal = -1f;
        }
        else {
            snappedHorizontal = 0f;
        }
        #endregion

        #region Snapped Vertical
        if(verticalMovement > 0f && verticalMovement < 0.55f) {
            snappedVertical = 0.5f;
        }
        else if(verticalMovement > 0.55f) {
            snappedVertical = 1f;
        }
        else if(verticalMovement < 0f && verticalMovement > -0.55f) {
            snappedVertical = -0.5f;
        } 
        else if(verticalMovement < -0.55f) {
            snappedVertical = -1f;
        }
        else {
            snappedVertical = 0f;
        }
        #endregion

        if(isSprinting) {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting) {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);  
    }
}
