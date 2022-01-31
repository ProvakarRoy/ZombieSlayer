using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float rotationSpeed = 4.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    private ThirdPersonAction playerControl;

    private Animator animator;

    [SerializeField] private GameObject bulletAsset;
    [SerializeField] private GameObject Gun;
    [SerializeField] private GameObject bulletParent;



    private void Awake()
    {
        playerControl = new ThirdPersonAction();
    }
    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 movement = playerControl.Player.Movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y).normalized;
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (playerControl.Player.Run.IsPressed())
            {
                playerSpeed = 10f;
                animator.SetFloat("Velocity", 1f);
            }
            else
            {
                animator.SetFloat("Velocity", 0.5f);
                playerSpeed = 5f;
            }
        }
        else
        {
            playerSpeed = 0f;
            animator.SetFloat("Velocity", 0f);
        }

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (playerControl.Player.Jump.triggered && groundedPlayer)
        {
            DoJump();
        }



        if (playerControl.Player.Attack.triggered)
        {
            DoAttack();
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void DoAttack()
    {
        GameObject bulletInstans = Instantiate(bulletAsset);
        bulletInstans.transform.position = Gun.transform.position;
        bulletInstans.transform.rotation = this.transform.rotation;
        bulletInstans.transform.parent = bulletParent.transform;
    }

    private void DoJump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}
