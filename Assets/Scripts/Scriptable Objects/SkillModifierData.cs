using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "SkillModifierData", menuName = "ScriptableObjects/SkillModifierData")]
    public class SkillModifierData : ScriptableObject
    {
        public float vitalityModifier;
        public float regenerationModifier;
        public float attackPowerModifier;
        public float criticalStrikeModifier;
        public float defenseModifier;
        public float agilityModifier;
    }
}