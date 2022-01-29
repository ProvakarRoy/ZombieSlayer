using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerDublicate : MonoBehaviour
{
    //input fields
    private ThirdPersonAction playerActionsAsset;
    private InputAction move;
    private Animator animator;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private GameObject Gun;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletParent;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonAction();
    }

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        //VelocityHash = Animator.StringToHash("Velocity");
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.Jump.started += DoJump;
        playerActionsAsset.Player.Run.started += DoRun;
        playerActionsAsset.Player.Attack.started += DoAttack;
        move = playerActionsAsset.Player.Movement;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Run.started -= DoRun;
        playerActionsAsset.Player.Attack.started -= DoAttack;
        playerActionsAsset.Player.Disable();
    }

    private void DoRun(InputAction.CallbackContext obj)
    {
        if (move.ReadValue<Vector2>() != new Vector2(0f, 0f)) 
        {
            maxSpeed += 10;
            //animator.SetFloat(VelocityHash, rb.velocity.magnitude / maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if(move.ReadValue<Vector2>() == Vector2.zero)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {

        }
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        Debug.Log("Attack");
        GameObject bulletInstans = Instantiate(bullet);
        bulletInstans.transform.position = Gun.transform.position;
        bulletInstans.transform.rotation = this.transform.rotation;
        bulletInstans.transform.parent = bulletParent.transform;
    }
}
