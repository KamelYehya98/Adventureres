using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;
        public Transform canvasTransform;

        [SerializeField]
        private PlayerCoreController coreController;

        private string _currentState;

        public bool facingHorizontal;
        public bool facingUp;
        public bool facingDown;

        private float _localScaleX;
        private bool _isMoving;
        private float _moveX;
        private float _moveY;

        public void Awake()
        {
            facingHorizontal = true;
            facingUp = false;
            facingDown = false;

            _localScaleX = 1f;
        }

        public void FixedUpdate()
        {
            transform.localScale = new Vector3(_localScaleX, 1, 1);
        }

        public void Update()
        {
            GetMovementInputVariables();
            ManageAnimationDirections();
            ManageAnimationStates();
        }

        private void GetMovementInputVariables()
        {
            _isMoving = animator.GetBool("IsMoving");
            _moveX = animator.GetFloat("MoveX");
            _moveY = animator.GetFloat("MoveY");
        }

        public void ManageAnimationStates()
        {
            if(coreController.animationManager.animator.GetBool("IsAttacking") == false)
            {
                if (_isMoving)
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
                    else
                    {
                        Debug.LogError("Failed to transition to idle");
                    }
                }
            }
        }

        private void ManageAnimationDirections()
        {
            if (_isMoving)
            {
                if (Math.Abs(_moveX) > Math.Abs(_moveY))
                {
                    facingHorizontal = true;
                    facingDown = false;
                    facingUp = false;

                    if(_moveX > 0)
                    {
                        _localScaleX = 1;
                    }
                    else if (_moveX < 0)
                    {
                        _localScaleX = -1;
                    }
                }
                else if (Math.Abs(_moveX) < Math.Abs(_moveY))
                {
                    facingHorizontal = false;

                    if (_moveY > 0)
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
        }

        public void SetLocalScaleX(float xScale)
        {
            _localScaleX = xScale;
        }

        public float GetLocalScaleX()
        {
            return _localScaleX;
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

        public void EndAttackAnimation()
        {
            animator.SetBool("IsAttacking", false);
        }

        public bool IsWeaponActiveInFrame()
        {
            return animator.GetFloat("Weapon.Active") == 1;
        }

        public bool CanFlipSidesInFrame()
        {
            return animator.GetFloat("Flip") == 1;
        }

        public bool IsAttackOpenInFrame()
        {
            return animator.GetFloat("AttackWindow.Open") > 0;
        }

        public void SetMovement(Vector2 movement)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
            animator.SetBool("IsMoving", movement != Vector2.zero);
        }

        public bool IsMoving()
        {
            return _isMoving;
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