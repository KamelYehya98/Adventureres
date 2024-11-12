using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerData playerData;

        public Rigidbody2D _rb;
        public SpriteRenderer _spriteRenderer;

        public AnimationManager animationManager;
        public WeaponComponent weaponComponent;

        public void Initialize(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public void Awake()
        {
            animationManager = GetComponent<AnimationManager>();
            weaponComponent = GetComponentInChildren<WeaponComponent>();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();

            DontDestroyOnLoad(gameObject);
        }

        public void Move(Vector2 direction)
        {
            if(animationManager.IsAttackState())
            {
                _rb.velocity = Vector2.zero;
            }
            else if(direction != null && playerData.Skills != null)
            {
                _rb.velocity = direction.normalized * playerData.Skills.Agility;
            }
        }

        public void TakeDamage(float damage) { }
    }


    public static class GenericAnimationStates
    {
        public const string IdleUp = "Idle";
        public const string IdleDown = "Idle";
        public const string IdleRight = "Idle";
        public const string WalkUp = "Running";
        public const string WalkDown = "Running";
        public const string WalkRight = "Running";
        public const string Attack1 = "Attack1";
        public const string Attack2 = "Attack2";
        public const string Attack3 = "Attack3";
    }
}

