using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;
        public Transform canvasTransform;

        private string _currentState;

        // Animation Control
        public bool facingHorizontal;
        public bool facingUp;
        public bool facingDown;
            
        public void Awake()
        {
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

        public void SetMovement(Vector2 movement)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
            animator.SetBool("IsMoving", movement != Vector2.zero);
        }

        public void MovementAnimation()
        {
            bool isMoving = animator.GetBool("IsMoving");
            float moveX = animator.GetFloat("MoveX");
            float moveY = animator.GetFloat("MoveY");

            if (isMoving)
            {
                if (Math.Abs(moveX) > Math.Abs(moveY))
                {
                    facingHorizontal = true;
                    facingDown = false;
                    facingUp = false;
                }
                else if (Math.Abs(moveX) < Math.Abs(moveY))
                {
                    facingHorizontal = false;
                    if (moveY > 0)
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

            if (moveX > 0)
            {
                Flip(true);
            }
            else if (moveX < 0)
            {
                Flip(false);
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

        void Flip(bool isRight)
        {
            Vector3 newScale = transform.localScale;

            if ((isRight && newScale.x < 0) || (!isRight && newScale.x > 0))
            {
                newScale.x *= -1;

                if (canvasTransform != null)
                {
                    Vector3 canvasScale = canvasTransform.localScale;
                    canvasScale.x *= -1;
                    canvasTransform.localScale = canvasScale;
                }
                else
                {
                    Debug.Log("Canvas transform is null");
                }
            }

            transform.localScale = newScale;
        }
    }
}
