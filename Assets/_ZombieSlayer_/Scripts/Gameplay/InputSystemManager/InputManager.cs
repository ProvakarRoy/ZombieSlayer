using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    ThirdPersonAction playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    public Vector2 movementInput;
    public Vector2 cameraInput;
    public float cameraInputX;
    public float cameraInputY;
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
            playerControls.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
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
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
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
