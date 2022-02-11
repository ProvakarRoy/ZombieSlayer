using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidLandInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public bool moveIsPressed = false;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool InverMouseY { get; private set; } = true;
    public float ZoomCameraInput { get; private set; } = 0.0f;
    public bool InvertScroll { get; private set; } = true;
    public bool CameraChangeWasPressedThisFrame { get; private set; } = false;
    public bool RunIsPressed { get; private set; } = false;
    public bool JumpIsPressed { get; private set; } = false;

    InputActions _input = null;

    private void OnEnable()
    {
        _input = new InputActions();
        _input.HumanoidLand.Enable();

        _input.HumanoidLand.Move.performed += SetMove;
        _input.HumanoidLand.Move.canceled += SetMove;

        _input.HumanoidLand.Look.performed += SetLook;
        _input.HumanoidLand.Look.canceled += SetLook;
        
        _input.HumanoidLand.ZoomCamera.started += SetZoomCamera;
        _input.HumanoidLand.ZoomCamera.canceled += SetZoomCamera;

        _input.HumanoidLand.Jump.performed += SetJump;
        _input.HumanoidLand.Jump.canceled += SetJump;

        _input.HumanoidLand.Run.started += SetRun;
        _input.HumanoidLand.Run.canceled += SetRun;
    }

    private void OnDisable()
    {
        _input.HumanoidLand.Move.performed -= SetMove;
        _input.HumanoidLand.Move.canceled -= SetMove;

        _input.HumanoidLand.Look.performed -= SetLook;
        _input.HumanoidLand.Look.canceled -= SetLook;

        _input.HumanoidLand.ZoomCamera.started -= SetZoomCamera;
        _input.HumanoidLand.ZoomCamera.canceled -= SetZoomCamera;

        _input.HumanoidLand.Jump.performed -= SetJump;
        _input.HumanoidLand.Jump.canceled -= SetJump;

        _input.HumanoidLand.Run.started -= SetRun;
        _input.HumanoidLand.Run.canceled -= SetRun;

        _input.HumanoidLand.Disable();
    }

    
    private void Update()
    {
        CameraChangeWasPressedThisFrame = _input.HumanoidLand.ChangeCamera.WasPressedThisFrame();
    }

    private void SetMove(InputAction.CallbackContext obj)
    {
        MoveInput = obj.ReadValue<Vector2>();
        moveIsPressed = !(MoveInput == Vector2.zero);
    }

    private void SetLook(InputAction.CallbackContext obj)
    {
        LookInput = obj.ReadValue<Vector2>();
    }

    private void SetRun(InputAction.CallbackContext obj)
    {
        RunIsPressed = obj.started;
    }

    private void SetJump(InputAction.CallbackContext obj)
    {
        JumpIsPressed = obj.performed;
    }

    private void SetZoomCamera(InputAction.CallbackContext obj)
    {
        ZoomCameraInput = obj.ReadValue<float>();
    }
}
