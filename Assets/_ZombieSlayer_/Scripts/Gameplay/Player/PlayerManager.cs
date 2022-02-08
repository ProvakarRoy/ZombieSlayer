using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    public bool isInteracting;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        inputManager = this.GetComponent<InputManager>();
        playerLocomotion = this.GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("IsInteraction");
        playerLocomotion.isJumping = animator.GetBool("IsJumping");
        animator.SetBool("IsGrounded", playerLocomotion.isGrounded);
    }
}
