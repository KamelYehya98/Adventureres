using Assets.Scripts.Managers;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class WarriorClass : PlayerClass
    {
        private PlayerClassData _data;
        private new void Start()
        {
            base.Start();
            Debug.Log("Entered Start at MageClass");

            _data = FindObjectOfType<ClassManager>().warriorData;

            Initialize(_data);
            InitializeComponents();
        }

        public override void Attack()
        {
            if (basicAttack != null && mana >= basicAttack.manaCost)
            {
                mana -= basicAttack.manaCost;
                basicAttack.Activate(gameObject);
                //Debug.Log($"{className} performs {basicAttack.abilityName}.");
            }
        }

        public override void SpecialAbility()
        {
            if (specialAbility != null && mana >= specialAbility.manaCost)
            {
                mana -= specialAbility.manaCost;
                specialAbility.Activate(gameObject);
                //Debug.Log($"{className} performs {specialAbility.abilityName}.");
            }
        }

        protected void InitializeComponents()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();

            _animator.runtimeAnimatorController = _data.animatorController;
        }
    }
}