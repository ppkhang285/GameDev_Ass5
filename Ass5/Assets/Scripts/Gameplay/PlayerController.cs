using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpHeight;
    private float gravity = -9.81f;

    public float attackCooldown;
    public float timeSinceLastAttack;

    private Ability ability;  // Special mechanics of the character
    public Animator animator;

    private CharacterController characterController;
    private Vector3 velocity;

    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        moveSpeed = GetComponent<Character>().Stats.movementSpeed;
        jumpHeight = GetComponent<Character>().Stats.jumpHeight;
        attackCooldown = GetComponent<Character>().Stats.attackCooldown;
        timeSinceLastAttack = attackCooldown; // ready for attack 
        ability = GetComponent<Character>().Stats.ability;
        animator = GetComponent<Character>().animator;
    }

    void Update()
    {
        ability.Passive(gameObject);
        HandleMovement();
        HandleAttack();
        // HandleJumpingAndGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            characterController.Move(move * moveSpeed * Time.deltaTime);

            animator.SetBool("isRunning", true);
        }
        else
            animator.SetBool("isRunning", false);
    }

    private void HandleJumpingAndGravity()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleAttack()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && timeSinceLastAttack >= attackCooldown)
            ability.Attack(gameObject);
    }
}
