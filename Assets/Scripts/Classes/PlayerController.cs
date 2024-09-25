using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerData playerData;

        public Rigidbody2D _rb;
        public SpriteRenderer _spriteRenderer;

        public AnimationManager animationManager;

        public void Initialize(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public void Start()
        {
            //animationManager = new AnimationManager();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();

           // animationManager.SetAnimator(GetComponent<Animator>());

            DontDestroyOnLoad(gameObject);
        }

        public void Move(Vector2 direction)
        {
            if(transform != null && direction != null && playerData.Skills != null)
            {
                transform.position += playerData.Skills.Agility * Time.deltaTime * (Vector3)direction;
                AdjustPlayerFacingDirection(direction);
            }
        }

        private void AdjustPlayerFacingDirection(Vector2 movement)
        {
            if (_spriteRenderer == null)
            {
                Debug.LogError("_spriteRenderer is null.");
                return;
            }

           // animationManager.ManageAnimations(_spriteRenderer, movement);
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
    }
}
