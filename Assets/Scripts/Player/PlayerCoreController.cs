using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCoreController : MonoBehaviour
    {
        public PlayerData playerData;

        public Rigidbody2D rb;
        public SpriteRenderer spriteRenderer;

        public AnimationManager animationManager;
        public WeaponController weaponComponent;
        public PlayerInvulnerability playerInvulnerability;

        public void Initialize(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero && playerData.Skills != null && !animationManager.IsAttackState())
            {
                Vector2 newPosition = rb.position + (direction.normalized * playerData.Skills.Agility * Time.fixedDeltaTime);

                rb.MovePosition(newPosition);
                animationManager.SetMovement(direction);
            }
            else
            {
                animationManager.SetMovement(Vector2.zero);
            }
        }

        public void OnAttackAddForceTrigger()
        {
            if (weaponComponent.weaponData.type == ItemType.Sword)
            {
                if (animationManager.facingDown)
                {
                    rb.AddForce(Vector2.down * weaponComponent.weaponData.attackForceOnPlayer[weaponComponent.currentComboIndex], ForceMode2D.Impulse);
                }
                else if (animationManager.facingUp)
                {
                    rb.AddForce(Vector2.up * weaponComponent.weaponData.attackForceOnPlayer[weaponComponent.currentComboIndex], ForceMode2D.Impulse);
                }
                else if (animationManager.facingHorizontal)
                {
                    Vector2 direction = transform.localScale.x == -1 ? Vector2.left : Vector2.right;

                    rb.AddForce(direction * weaponComponent.weaponData.attackForceOnPlayer[weaponComponent.currentComboIndex], ForceMode2D.Impulse);
                }
            }
        }

        public void OnAttackStopForceTrigger()
        {
            rb.velocity = Vector2.zero;
        }

        public void TakeDamage(float damage) 
        {
            StartCoroutine(playerInvulnerability.StartInvulnerability());
        }

        public Vector2 GetFacingForce()
        {
            if (animationManager.facingDown)
                return Vector2.down;
            else if (animationManager.facingUp)
                return Vector2.up;
            else if (animationManager.facingHorizontal && transform.localScale.x == -1)
                return Vector2.left;
            else if (animationManager.facingHorizontal && transform.localScale.x == 1)
                return Vector2.right;

            else return Vector2.zero;
        }
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

