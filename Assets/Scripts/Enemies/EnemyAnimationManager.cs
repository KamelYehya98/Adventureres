using Assets.Scripts.Enemiies;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyAnimationManager : MonoBehaviour
    {

        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;

        private string _currentState;

        // Animation Control
        public bool facingHorizontal;
        public bool facingUp;
        public bool facingDown;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();

            facingHorizontal = true;
            facingUp = false;
            facingDown = false;
        }

        public void ChangeAnimatorState(string newState)
        {
            if (animator != null)
            {
                if (_currentState == newState)
                {
                    return;
                }

                animator.Play(newState);

                _currentState = newState;
            }
        }

        public void TakeDamage()
        {
            if (facingHorizontal)
            {
                ChangeAnimatorState(GenericEnemyAnimationStates.DamageRight);
            }
            else if (facingDown)
            {
                ChangeAnimatorState(GenericEnemyAnimationStates.DamageDown);
            }
            else if (facingUp)
            {
                ChangeAnimatorState(GenericEnemyAnimationStates.DamageUp);
            }
        }

        public void Die()
        {
            ChangeAnimatorState(GenericEnemyAnimationStates.Death);
        }

        public void MovementAnimation()
        {
            bool isMoving = rb.velocity != Vector2.zero;

            if (isMoving)
            {
                if (Math.Abs(rb.velocity.x) > Math.Abs(rb.velocity.y))
                {
                    facingHorizontal = true;
                    facingDown = false;
                    facingUp = false;
                }
                else if (Math.Abs(rb.velocity.x) < Math.Abs(rb.velocity.y))
                {
                    facingHorizontal = false;
                    if (rb.velocity.y > 0)
                    {
                        facingUp = true;
                        facingDown = false;
                    }
                    else
                    {
                        facingDown = true;
                        facingUp = false;
                    }
                }
            }

            if (rb.velocity.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.x < 0)
            {
                spriteRenderer.flipX = true;
            }

            if (isMoving)
            {
                if (facingHorizontal)
                {
                    ChangeAnimatorState(GenericEnemyAnimationStates.WalkRight);
                }
                else
                {
                    if (facingDown)
                    {
                        ChangeAnimatorState(GenericEnemyAnimationStates.WalkDown);
                    }
                    else if (facingUp)
                    {
                        ChangeAnimatorState(GenericEnemyAnimationStates.WalkUp);
                    }
                }
            }
            else
            {
                if (facingHorizontal)
                {
                    ChangeAnimatorState(GenericEnemyAnimationStates.IdleRight);
                }
                else if (facingDown)
                {
                    ChangeAnimatorState(GenericEnemyAnimationStates.IdleDown);
                }
                else if (facingUp)
                {
                    ChangeAnimatorState(GenericEnemyAnimationStates.IdleUp);
                }
            }
        }

    }
}
