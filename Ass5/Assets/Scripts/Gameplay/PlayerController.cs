using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement settings
    public float MoveSpeed { get; private set; }
    public float JumpHeight { get; private set; }
    private float gravity = -9.81f;

    // Components
    private Transform character;
    private CharacterController characterController;
    private Vector3 velocity;

    // Is the player grounded?
    private bool isGrounded;

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        MoveSpeed = GetComponent<Character>().stats.movementSpeed;
        JumpHeight = GetComponent<Character>().stats.jumpHeight;
    }

    void Update()
    {
        // Handle player movement
        HandleMovement();

        // Handle jumping and gravity
        // HandleJumpingAndGravity();
    }

    private void HandleMovement()
    {
        // Get input for movement (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a vector for movement relative to the character's orientation
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply movement
        characterController.Move(move * MoveSpeed * Time.deltaTime);
    }

    private void HandleJumpingAndGravity()
    {
        // Check if the player is grounded using CharacterController's built-in property
        isGrounded = characterController.isGrounded;

        // If the player is grounded and moving downward, stop downward velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small value to keep the character grounded
        }

        // If the player is grounded and the jump button is pressed, apply jump force
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity); // Jump formula
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player based on calculated velocity
        characterController.Move(velocity * Time.deltaTime);
    }
}
