using UnityEngine;

public class HumanoidlandController : MonoBehaviour
{
    public Transform CameraFollow;

    Rigidbody _rigidbody = null;
    CapsuleCollider _capsuleCollider = null;
    [SerializeField] HumanoidLandInput _input;
    [SerializeField] CameraController _CameraController;

    Vector3 _playerMoveInput = Vector3.zero;

    Vector3 _playerLookInput = Vector3.zero;
    Vector3 _previousPlayerLookInput = Vector3.zero;
    float cameraPitch = 0.0f;
    [SerializeField] float _playerLookInputLerpTime = 0.35f;

    [Header("Movement")]
    [SerializeField] float _movementMultiplier = 30.0f;
    [SerializeField] float _rotationSpeedMultiplier = 200.0f;
    [SerializeField] float _pitchSpeedMultiplier = 200.0f;
    [SerializeField] float _runMultiplier = 2.5f;
    [SerializeField] float _jumpMultiplier = 200.0f;

    [Header("Ground Check")]
    [SerializeField] bool _playerIsGrounded = true;
    [SerializeField] [Range(0.0f, 1.8f)] float _groundCheckRadiusMultiplier = 0.9f;
    [SerializeField] [Range(-0.95f, 1.05f)] float _groundCheckDistance = 0.05f;
    RaycastHit _groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float _gravityFallCurrent = -100.0f;
    [SerializeField] float _gravityFallMin = -100.0f;
    [SerializeField] float _gravityFallMax = -500.0f;
    [SerializeField] [Range(-5.0f, -35.0f)] float _gravityFallIncrementAmount = -20.0f;
    [SerializeField] float _gravityFallIncrementTime = 0.05f;
    [SerializeField] float _playerFallTime = 0.0f;
    [SerializeField] float _gravity = 0.0f;

    [Header("Camera")]
    [SerializeField] private float minMoveClamp = -89.9f;
    [SerializeField] private float maxMoveClamp = 89.9f;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if(!_CameraController.UsingOrbitalCamera)
        {
            _playerLookInput = GetLookInput();
            PlayerLook();
            PitchCamera();
        }

        _playerMoveInput = GetMoveInput();
        _playerIsGrounded = PlayerGroundCheck();
        _playerMoveInput.y = PlayerGravity();
        _playerMoveInput = PlayerRun();
        _playerMoveInput = PlayerJump();

        _playerMoveInput = PlayerMove();

        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);
    }

    private Vector3 GetLookInput()
    {
        _previousPlayerLookInput = _playerLookInput;
        _playerLookInput = new Vector3(_input.LookInput.x, (_input.InverMouseY ? -_input.LookInput.y : _input.LookInput.y), 0.0f);
        return Vector3.Lerp(_previousPlayerLookInput, _playerLookInput * Time.deltaTime, _playerLookInputLerpTime);
    }

    private void PlayerLook()
    {
        _rigidbody.rotation = Quaternion.Euler(0.0f, _rigidbody.rotation.eulerAngles.y + (_playerLookInput.x * _rotationSpeedMultiplier), 0.0f);
    }

    private void PitchCamera()
    {
        Vector3 rotationValues = CameraFollow.rotation.eulerAngles;
        cameraPitch += _playerLookInput.y * _pitchSpeedMultiplier;
        cameraPitch = Mathf.Clamp(cameraPitch, minMoveClamp, maxMoveClamp);

        CameraFollow.rotation = Quaternion.Euler(cameraPitch, rotationValues.y, rotationValues.z);
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
    }

    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = _capsuleCollider.radius * _groundCheckRadiusMultiplier;
        float sphereCastTravelDistance = _capsuleCollider.bounds.extents.y - sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_rigidbody.position, sphereCastRadius, Vector3.down, out _groundCheckHit, sphereCastTravelDistance);
    }

    private float PlayerGravity()
    {
        if (_playerIsGrounded)
        {
            _gravity = 0.0f;
            _gravityFallCurrent = _gravityFallMin;
        }
        else
        {
            _playerFallTime -= Time.fixedDeltaTime;
            if (_playerFallTime < 0.0f)
            {
                if (_gravityFallCurrent > _gravityFallMax)
                {
                    _gravityFallCurrent += _gravityFallIncrementAmount;
                }
                _playerFallTime = _gravityFallIncrementTime;
                _gravity = _gravityFallCurrent;
            }
        }
        return _gravity;
    }

    private Vector3 PlayerRun()
    {
        Vector3 calculatedPlayerRunScript = _playerMoveInput;
        if (_input.RunIsPressed)
        {
            calculatedPlayerRunScript.x *= _runMultiplier;
            calculatedPlayerRunScript.z *= _runMultiplier;
        }
        return calculatedPlayerRunScript;
    }

    private Vector3 PlayerJump()
    {
        Vector3 calculatedPlayerJumpScript = _playerMoveInput;
        if (_input.JumpIsPressed)
        {
            calculatedPlayerJumpScript.y += _jumpMultiplier;
        }
        return calculatedPlayerJumpScript;
    }

    private Vector3 PlayerMove()
    {
        Vector3 calculatedPlayerMovement = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass,
                                        _playerMoveInput.y * _rigidbody.mass,
                                        _playerMoveInput.z * _movementMultiplier * _rigidbody.mass));

        return calculatedPlayerMovement;
    }
}
