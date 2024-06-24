using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 1f;

    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (_movement != Vector2.zero)
        {
            Move();
            AdjustPlayerFacingDirection(_movement);
        }

        AnimatePlayer(_movement);
    }
    #region Utility Methods

    private void OnMove(Vector2 direction)
    {
        _movement = direction;
    }

    private void Move()
    {
        transform.position += new Vector3(_movement.x, _movement.y, 0) * movementSpeed * Time.deltaTime;
    }

    private void AdjustPlayerFacingDirection(Vector2 movement)
    {
        if (movement.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (movement.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void AnimatePlayer(Vector2 direction)
    {
        _animator.SetFloat("moveX", direction.x);
        _animator.SetFloat("moveY", direction.y);


        _animator.SetBool("moveUp", direction.y > 0);
        _animator.SetBool("moveDown", direction.y < 0);
        _animator.SetBool("isMoving", direction != Vector2.zero);
    }
    #endregion
    #region Subscribing To Event
    private void OnEnable()
    {
        PlayerInputInvoker.PlayerMovement += OnMove;
    }
    private void OnDisable()
    {
        PlayerInputInvoker.PlayerMovement += OnMove;
    }
    #endregion
}