using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;

    private PlayerControls playerControls;
    public Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public bool isMoving;
    public bool moveUp;
    public bool moveDown;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
        AdjustPlayerFacingDirection(movement);

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);

        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x) && movement.y > 0)
        {
            moveUp = true; 
        }

        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x) && movement.y < 0)
        {
            moveDown = true;
        }

        if (Mathf.Abs(movement.y) <= Mathf.Abs(movement.x) || !isMoving)
        {
            moveUp = false;
            moveDown = false;
        }

        if (movement.x > -0.1 && movement.x < 0.1 && movement.y > -0.1 && movement.y < 0.1)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        animator.SetBool("moveUp", moveUp);
        animator.SetBool("moveDown", moveDown);
        animator.SetBool("isMoving", isMoving);
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void AdjustPlayerFacingDirection(Vector2 movement)
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(movement.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (movementSpeed * Time.fixedDeltaTime));
    }
}