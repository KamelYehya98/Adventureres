using Assets.Scripts.Classes;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AnimationManager
    {
        public Animator animator;
        private string _currentState;

        // Animation Control
        public bool facingHorizontal;
        public bool facingUp;
        public bool facingDown;

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

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }

        public void ManageAnimations(SpriteRenderer spriteRenderer, Vector2 movement)
        {
            bool isMoving = movement != Vector2.zero;

            if (Math.Abs(movement.x) > Math.Abs(movement.y))
            {
                facingHorizontal = true;
                facingDown = false;
                facingUp = false;
            }
            else if (Math.Abs(movement.x) < Math.Abs(movement.y))
            {
                facingHorizontal = false;
                if (movement.y > 0)
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

            if (movement.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < 0)
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
