using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string TargetAnimation, bool isInteracting)
    {
        animator.SetBool("IsInteraction", isInteracting);
        animator.CrossFade(TargetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float HorizontalMovement, float VerticalMovement, bool isSprinting)
    {
        //Animation Snapping
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (HorizontalMovement > 0 && HorizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (HorizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if(HorizontalMovement < 0 && HorizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if(HorizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snapped Vertical
        if (VerticalMovement > 0 && VerticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (VerticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (VerticalMovement < 0 && VerticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (VerticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if(isSprinting)
        {
            snappedHorizontal = HorizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
