using Assets.Scripts.Classes;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AnimationManager : MonoBehaviour
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

        public bool IsAttackState()
        {
            return animator.GetBool("IsAttacking");
        }

        public void StartAttackAnimation(string attackName)
        {
            animator.SetBool("IsAttacking", true);
            ChangeAnimatorState(attackName);
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
                    ChangeAnimatorState(GenericAnimationStates.WalkRight);
                }
                else
                {
                    if (facingDown)
                    {
                        ChangeAnimatorState(GenericAnimationStates.WalkDown);
                    }
                    else if (facingUp)
                    {
                        ChangeAnimatorState(GenericAnimationStates.WalkUp);
                    }
                }
            }
            else
            {
                if (facingHorizontal)
                {
                    ChangeAnimatorState(GenericAnimationStates.IdleRight);
                }
                else if (facingDown)
                {
                    ChangeAnimatorState(GenericAnimationStates.IdleDown);
                }
                else if (facingUp)
                {
                    ChangeAnimatorState(GenericAnimationStates.IdleUp);
                }
            }
        }
    }
}
