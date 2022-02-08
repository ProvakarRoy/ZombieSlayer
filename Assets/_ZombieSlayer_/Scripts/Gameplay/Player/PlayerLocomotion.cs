using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Script Refrences")]
    PlayerManager playerManager;
    PlayerAnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("onAir Flags")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;


    private void Awake()
    {
        inputManager = this.GetComponent<InputManager>();
        playerManager = this.GetComponent<PlayerManager>();
        animatorManager = this.GetComponent<PlayerAnimatorManager>();
        playerRigidbody = this.GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        //if (playerManager.isInteracting)
        //    return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (isJumping)
            return;

        moveDirection = this.transform.forward * inputManager.verticalInput;
        moveDirection = moveDirection + this.transform.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        if(isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount <= 0.5f)
            {
                moveDirection = moveDirection * walkingSpeed;
            }
            else
            {
                moveDirection = moveDirection * runningSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;
        if (inputManager.verticalInput == 1)
        {
            targetDirection = cameraObject.forward * inputManager.verticalInput;
            targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = this.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            transform.rotation = playerRotation;
        }
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = this.transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        targetPosition = this.transform.position;
        if(!isGrounded && !isJumping)
        {
            if(!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            //playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }
            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
         }
        else
        {
            isGrounded = false;
        }

        if(isGrounded && !isJumping)
        {
            if(playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJump()
    {
        if(isGrounded)
        {
            animatorManager.animator.SetBool("IsJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            playerRigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.VelocityChange);

            //float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            //Vector3 playerVelocity = moveDirection;
            //playerVelocity.y = jumpingVelocity;
            //playerRigidbody.velocity = playerVelocity;
        }
    }


}
