using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private float currentSpeed;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Movement direction
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        playerAnimator.SetFloat("Velocity", move.magnitude * (currentSpeed / runSpeed));
        Debug.Log(playerAnimator.GetFloat("Velocity"));

        // Move player
        playerRb.linearVelocity = new Vector3(
            move.x * currentSpeed,
            playerRb.linearVelocity.y,
            move.z * currentSpeed
        );

        // Rotate player toward movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
            currentSpeed = runSpeed;
        else
            currentSpeed = moveSpeed;
    }

    void OnJump(InputValue value)
    {
        playerRb.AddForce(new Vector3(0, 3f, 0), ForceMode.Impulse);
        playerAnimator.SetBool("canJump", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("canJump", false);
        }
    }
}