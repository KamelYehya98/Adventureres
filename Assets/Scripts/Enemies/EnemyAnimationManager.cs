using Assets.Scripts.Enemiies;
using UnityEngine;
using System;

namespace Assets.Scripts.Enemies
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyControllerBase controllerBase;

        public SpriteRenderer spriteRenderer;
        public Animator animator;

        private string _currentState;

        public bool facingHorizontal;
        public bool facingUp;
        public bool facingDown;

        private bool _isMoving;

        public void Awake()
        {
            facingHorizontal = true;
            facingUp = false;
            facingDown = false;

            _isMoving = false;
        }

        public void FixedUpdate()
        {
            CheckIsMoving();
        }

        public void Update()
        {
            if(!controllerBase.movementController.isDead)
            {
                ManageAnimationStates();
                ManageAnimationDirections();
            }
        }

        public void ChangeAnimatorState(string newState)
        {
            if (animator != null)
            {
                if (_currentState == newState)
                {
                    return;
                }

                animator.StopPlayback();
                animator.Play(newState);

                _currentState = newState;
            }
        }

        public void TakeDamageAnimation()
        {
            if (!controllerBase.movementController.isDead)
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
        }

        public void Die()
        {
            ChangeAnimatorState(GenericEnemyAnimationStates.Death);
        }

        private void ManageAnimationStates()
        {
            if (_isMoving)
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

        private void CheckIsMoving()
        {
            _isMoving = controllerBase.movementController.rb.velocity != Vector2.zero;
        }

        private void ManageAnimationDirections()
        {
            if (_isMoving)
            {
                if (Math.Abs(controllerBase.movementController.rb.velocity.x) > Math.Abs(controllerBase.movementController.rb.velocity.y))
                {
                    facingHorizontal = true;
                    facingDown = false;
                    facingUp = false;
                }
                else if (Math.Abs(controllerBase.movementController.rb.velocity.x) < Math.Abs(controllerBase.movementController.rb.velocity.y))
                {
                    facingHorizontal = false;
                    if (controllerBase.movementController.rb.velocity.y > 0)
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

            if (controllerBase.movementController.rb.velocity.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (controllerBase.movementController.rb.velocity.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}
