using Assets.Scripts.Managers;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Skills;
using System;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public abstract class PlayerClass : MonoBehaviour
    {
        public string className;  // Made public for easier debugging

        protected float mana;

        protected PlayerSkills skills;

        public Ability basicAttack;
        public Ability specialAbility;

        protected Rigidbody2D _rb;
        protected SpriteRenderer _spriteRenderer;

        protected PlayerClassManager playerClassManager;
        protected ClassManager classManager;
        protected AnimationManager animationManager;

        public void Initialize(PlayerClassData data)
        {
            Debug.Log("Entered Inititalize at playerClass");

            if (data == null)
            {
                Debug.LogError("PlayerClassData is null during initialization.");
                return;
            }

            className = data.className;

            skills = new PlayerSkills()
            {
                vitality = data.vitality,
                criticalStrike = data.criticalStrike,
                agility = data.agility,
                attackPower = data.attackPower,
                defense = data.defense,
                regeneration = data.regeneration
            };

            mana = data.mana;
            basicAttack = data.basicAttack;
            specialAbility = data.specialAbility;
        }

        public void Start()
        {
            animationManager = new AnimationManager();

            if (!TryGetComponent(out playerClassManager))
            {
                Debug.LogError("PlayerClassManager not found.");
            }
        }

        public ClassState SaveState()
        {
            return new ClassState
            {
                vitality = skills.vitality,
                criticalStrike = skills.criticalStrike,
                agility = skills.agility,
                attackPower = skills.attackPower,
                className = className,
                defense = skills.defense,
                regeneration = skills.regeneration,
                mana = mana,
                basicAttack = basicAttack,
                specialAbility = specialAbility,
            };
        }

        public void LoadState(ClassState state)
        {
            if (state == null)
            {
                Debug.LogError("ClassState is null.");
                return;
            }

            mana = state.mana;
            basicAttack = state.basicAttack;
            specialAbility = state.specialAbility;
            className = state.className;
            skills.regeneration = state.regeneration;
            skills.vitality = state.vitality;
            skills.criticalStrike = state.criticalStrike;
            skills.vitality = state.vitality;
            skills.attackPower = state.attackPower;
            skills.defense = state.defense;
        }

        public void Move(Vector2 direction)
        {
            if(transform != null && direction != null && skills != null)
            {
                transform.position += skills.agility * Time.deltaTime * (Vector3)direction;
                AdjustPlayerFacingDirection(direction);
            }
        }

        private void OnSwitchClass(int direction)
        {
            playerClassManager.SwitchClass(direction);
        }

        private void AdjustPlayerFacingDirection(Vector2 movement)
        {
            if (_spriteRenderer == null)
            {
                Debug.LogError("_spriteRenderer is null.");
                return;
            }

            animationManager.ManageAnimations(_spriteRenderer, movement);
        }

        public void TakeDamage(float damage) { }

        public abstract void Attack();
        public abstract void SpecialAbility();
    }


    public static class GenericAnimationStates
    {
        public const string IdleUp = "wolf_idle_up";
        public const string IdleDown = "wolf_idle_down";
        public const string IdleRight = "wolf_idle_right";
        public const string WalkUp = "wolf_walk_up";
        public const string WalkDown = "wolf_walk_down";
        public const string WalkRight = "wolf_walk_right";
    }
}

