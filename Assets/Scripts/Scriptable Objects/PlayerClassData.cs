using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerClassData", menuName = "ScriptableObjects/PlayerClassData")]
    public class PlayerClassData : ScriptableObject
    {
        public string className;
        public float vitality;
        public float regeneration;
        public float attackPower;
        public float criticalStrike;
        public float defense;
        public float agility;
        public float mana;
        public SkillModifierData skillModifier;
        public Ability basicAttack;
        public Ability specialAbility;
        public GameObject prefab;
        public RuntimeAnimatorController animatorController;
        public PlayerClass classComponent;
        public System.Type classType;
    }
}