using UnityEngine;

public abstract class PlayerClass : MonoBehaviour
{
    private string className;
    private int health;
    private int mana;
    private int attackPower;
    private int defense;

    public Ability basicAttack;
    public Ability specialAbility;

    protected PlayerControls inputActions;
    protected Vector2 moveInput;
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    public void Initialize(PlayerClassData data)
    {
        className = data.className;
        health = data.health;
        mana = data.mana;
        attackPower = data.attackPower;
        defense = data.defense;
        basicAttack = data.basicAttack;
        specialAbility = data.specialAbility;

        inputActions = new PlayerControls();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        
    }
    protected void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            Move();
            AdjustPlayerFacingDirection(moveInput);
        }

        AnimatePlayer(moveInput);
    }

    private void OnMove(Vector2 direction)
    {
        moveInput = direction;
    }

    private void Move()
    {
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * 1f * Time.deltaTime;
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
    public abstract void Attack();
    public abstract void SpecialAbility();

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{className} has died.");
    }
}