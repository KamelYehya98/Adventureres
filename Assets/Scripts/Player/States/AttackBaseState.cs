using Assets.Scripts.Enemiies;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class AttackBaseState : PlayerStateBase
    {
        // How long this state should be active for before moving on
        public float duration;
        // Cached animator component
        // bool to check whether or not the next attack in the sequence should be played or not
        protected bool shouldCombo;
        // The attack index in the sequence of attacks
        protected int attackIndex;


        // The cached hit collider component of this attack
        protected Collider2D hitCollider;
        // Cached already struck objects of said attack to avoid overlapping attacks on same target
        private List<Collider2D> collidersDamaged;
        // The Hit Effect to Spawn on the afflicted Enemy
        private GameObject HitEffectPrefab;
        private SpriteRenderer playerSpriteRenderer;
        protected WeaponController weaponController;

        // Input buffer Timer
        protected float AttackPressedTimer = 0;

        private void OnPlayerSpriteChanged(SpriteRenderer spriteRenderer)
        {
            Sprite newSprite = spriteRenderer.sprite;
            // Notify the weapon component to update its animation
            weaponController.OnPlayerSpriteChanged(newSprite);
        }

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            collidersDamaged = new List<Collider2D>();

            playerSpriteRenderer = GetComponent<SpriteRenderer>();

            playerSpriteRenderer.RegisterSpriteChangeCallback(OnPlayerSpriteChanged);

            //hitCollider = playerController.GetComponentInChildren<Collider2D>();

            HitEffectPrefab = GetComponent<CharacterStateManager>().Hiteffect;
            weaponController = GetComponent<CharacterStateManager>().WeaponController;
            hitCollider = GetComponent<CharacterStateManager>().hitbox;

            shouldCombo = false;
            AttackPressedTimer = 0;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (animationManager.animator.GetFloat("Weapon.Active") == 1f)
            {
                Attack();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Decrease the input buffer timer
            AttackPressedTimer -= Time.deltaTime;

            if (animationManager.animator.GetFloat("Flip") == 1f && IsChangingDirection())
            {
                stateMachine.SetNextState(new IdleCombatState());
                OnExit();
            }
            // Attack if the weapon is active

            // Check if the attack input is pressed
            if (inputController.attackInput > 0)
            {
                AttackPressedTimer = 1.5f;  // Reset buffer timer when attack input is detected
            }
        }

        public bool IsChangingDirection()
        {
            if (
                    inputController.moveInput != Vector2.zero && 
                    (
                        (animationManager.facingDown && inputController.moveInput.y > 0) ||
                        (animationManager.facingUp && inputController.moveInput.y < 0) ||
                        (animationManager.facingHorizontal && playerController.transform.localScale.x == -1 && inputController.moveInput.x > 0) ||
                        (animationManager.facingHorizontal && playerController.transform.localScale.x == 1 && inputController.moveInput.x < 0)
                    )
               )
            {
                return true;
            }

            return false;
        }
        public override void OnExit()
        {
            base.OnExit();

            weaponController.ResetAttackIndex();

            playerSpriteRenderer.UnregisterSpriteChangeCallback(OnPlayerSpriteChanged);

            weaponController.GetComponent<SpriteRenderer>().sprite = null;

            animationManager.animator.SetBool("IsAttacking", false);
        }

        protected void Attack()
        {
            Collider2D[] collidersToDamage = new Collider2D[1000];

            ContactFilter2D filter = new()
            {
                useTriggers = true
            };

            int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);

            for (int i = 0; i < colliderCount; i++)
            {
                if (!collidersDamaged.Contains(collidersToDamage[i]))
                {
                    TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                    // Only check colliders with a valid Team Componnent attached
                    if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                    {
                        //Object.Instantiate(HitEffectPrefab, collidersToDamage[i].transform);

                        if (collidersToDamage != null && collidersToDamage[i].GetComponentInParent<Enemy>() != null)
                        {
                            collidersToDamage[i].GetComponentInParent<Enemy>().KnockBackEffect(playerController.GetFacingForce() * weaponController.weaponData.attackForceOnOthers[weaponController.currentComboIndex]);
                            collidersToDamage[i].GetComponentInParent<Enemy>().TakeDamage(10);
                        }

                        Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");

                        collidersDamaged.Add(collidersToDamage[i]);
                    }
                }
            }
        }
    }
}