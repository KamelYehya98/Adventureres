using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class MeleeBaseState : State
    {
        // How long this state should be active for before moving on
        public float duration;
        // Cached animator component
        protected Animator animator;
        // bool to check whether or not the next attack in the sequence should be played or not
        protected bool shouldCombo;
        // The attack index in the sequence of attacks
        protected int attackIndex;
        protected PlayerInputController inputController;


        // The cached hit collider component of this attack
        protected Collider2D hitCollider;
        // Cached already struck objects of said attack to avoid overlapping attacks on same target
        private List<Collider2D> collidersDamaged;
        // The Hit Effect to Spawn on the afflicted Enemy
        private GameObject HitEffectPrefab;

        // Input buffer Timer
        protected float AttackPressedTimer = 0;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            animator = GetComponent<Animator>();
            collidersDamaged = new List<Collider2D>();
            hitCollider = GetComponent<ComboCharacter>().hitbox;
            HitEffectPrefab = GetComponent<ComboCharacter>().Hiteffect;
            inputController = GetComponent<PlayerInputController>();
            shouldCombo = false;
            AttackPressedTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // Decrease the input buffer timer
            AttackPressedTimer -= Time.deltaTime;

            // Attack if the weapon is active
            if (animator.GetFloat("Weapon.Active") > 0f)
            {
                Attack();
            }

            // Check if the attack input is pressed
            if (inputController.attackInput == 1)
            {
                AttackPressedTimer = 2;  // Reset buffer timer when attack input is detected
            }
        }



        public override void OnExit()
        {
            base.OnExit();
        }

        protected void Attack()
        {
            Collider2D[] collidersToDamage = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;
            int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);
            for (int i = 0; i < colliderCount; i++)
            {

                if (!collidersDamaged.Contains(collidersToDamage[i]))
                {
                    TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                    // Only check colliders with a valid Team Componnent attached
                    if (hitTeamComponent && hitTeamComponent.teamIndex == TeamIndex.Enemy)
                    {
                        GameObject.Instantiate(HitEffectPrefab, collidersToDamage[i].transform);
                        Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");
                        collidersDamaged.Add(collidersToDamage[i]);
                    }
                }
            }
        }

    }
}
