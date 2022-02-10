using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    ThirdPersonAction playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    public bool run_Input;
    public bool jump_Input;

    private void Awake()
    {
        animatorManager = this.GetComponent<PlayerAnimatorManager>();
        playerLocomotion = this.GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new ThirdPersonAction();
            playerControls.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Movement.canceled += i => movementInput = Vector2.zero;
            playerControls.Player.Run.performed += i => run_Input = true;
            playerControls.Player.Run.canceled += i => run_Input = false;
            playerControls.Player.Jump.performed += i => jump_Input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
        HandleSprinting();
    }
    private void HandleMovementInput()
    {
        float horizontalAnimatedMovement;
        float verticalAnimatedMovement;
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        horizontalAnimatedMovement = horizontalInput;
        verticalAnimatedMovement = verticalInput;
        animatorManager.UpdateAnimatorValues(horizontalInput, 0, playerLocomotion.isSprinting);
        animatorManager.UpdateAnimatorValues(horizontalInput, verticalInput, playerLocomotion.isSprinting);
    }
    private void HandleSprinting()
    {
        if (run_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }
    private void HandleJumpingInput()
    {
        if(jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJump();
        }
    }
}
