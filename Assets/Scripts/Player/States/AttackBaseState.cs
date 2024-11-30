using Assets.Scripts.Enemiies;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class AttackBaseState : PlayerStateBase
    {
        public float duration;

        protected bool shouldCombo;
        protected int attackIndex;
        protected float AttackPressedTimer = 0;

        private List<Collider2D> collidersDamaged;
        private readonly float _attackBufferTime = 1f;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            collidersDamaged = new List<Collider2D>();

            AttackPressedTimer = 0;
            shouldCombo = false;

            RegisterSprites();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            CanWeaponnAttack();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            DecrementAttackTimer();
            CheckIfShouldCombo();
            CheckIfPlayerCanFlipDirections();
            ResetAttackBufferOnAttack();
        }

        public override void OnExit()
        {
            base.OnExit();

            ResetAttackIndex();
            UnregisterSprites();
            EndAttackAnimation();
        }

        public void UnregisterSprites()
        {
            playerStateManager.spriteRendrer.UnregisterSpriteChangeCallback(OnPlayerSpriteChanged);

            playerStateManager.coreController.weaponController.weaponSpriteRenderer.sprite = null;
        }

        public void RegisterSprites()
        {
            playerStateManager.spriteRendrer.RegisterSpriteChangeCallback(OnPlayerSpriteChanged);
        }

        public void CheckIfPlayerCanFlipDirections()
        {
            if (playerStateManager.coreController.animationManager.CanFlipSidesInFrame() && playerStateManager.coreController.movementController.IsChangingDirection())
            {
                stateMachine.SetNextState(new IdleState());
                OnExit();
            }
        }
        
        public void DecrementAttackTimer()
        {
            AttackPressedTimer -= Time.deltaTime;
        }

        public void ResetAttackBufferOnAttack()
        {
            if (playerStateManager.coreController.inputController.GetAttackInput() > 0)
            {
                AttackPressedTimer = 1.5f;
                //playerStateManager.coreController.animationManager.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            }
        }

        public void CheckIfShouldCombo()
        {
            if (playerStateManager.coreController.animationManager.IsAttackOpenInFrame() && playerStateManager.coreController.inputController.GetAttackInput() == 1)
            {
                shouldCombo = true;
                AttackPressedTimer = 0;
                playerStateManager.coreController.inputController.ResetAttackInput();
            }
        }

        public void CanWeaponnAttack()
        {
            if (playerStateManager.coreController.animationManager.IsWeaponActiveInFrame())
            {
                Attack();
            }
        }

        private void OnPlayerSpriteChanged(SpriteRenderer spriteRenderer)
        {
            Sprite newSprite = spriteRenderer.sprite;
            playerStateManager.coreController.weaponController.OnPlayerSpriteChanged(newSprite);
        }

        protected void Attack()
        {
            Collider2D[] collidersToDamage = new Collider2D[100];

            ContactFilter2D filter = new()
            {
                useTriggers = true
            };

            int colliderCount = Physics2D.OverlapCollider(playerStateManager.hitbox, filter, collidersToDamage);

            for (int i = 0; i < colliderCount; i++)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i]))
                {
                    TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                    if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                    {
                        if (collidersToDamage != null && collidersToDamage[i].GetComponentInParent<EnemyControllerBase>() != null)
                        {
                            ApplyAttackEffects(collidersToDamage[i].GetComponentInParent<EnemyControllerBase>());
                        }

                        Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");

                        collidersDamaged.Add(collidersToDamage[i]);
                    }
                }
            }
        }

        private float CalculatePlayerAttackDamage()
        {
            return 10f;
        }

        private void ApplyAttackEffects(EnemyControllerBase enemy)
        {
            int currentComboIndex = playerStateManager.coreController.weaponController.currentComboIndex;

            Vector2 playerFacingDirection = playerStateManager.coreController.movementController.GetFacingDirection();
            float weaponKnockBackForce = playerStateManager.coreController.weaponController.weaponData.attackForceOnOthers[currentComboIndex];

            enemy.KnockBackEffect(playerFacingDirection * weaponKnockBackForce);
            enemy.TakeDamage(CalculatePlayerAttackDamage());
        }
    }
}