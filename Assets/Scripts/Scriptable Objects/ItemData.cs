using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemData : ScriptableObjectBase
    {

        [Header("Only Gameplay")]
        public ItemType type;
        public ActionType actionType;
        public Vector2Int range = new(5, 4);


        [Header("Only UI")]
        public bool stackable = true;

        [Header("Both")]
        [JsonIgnore]
        public Sprite image;

        [field: SerializeField] public ComboAnimations[] comboAnimations;
    }

    public enum ItemType
    {
        None,
        Loot,
        Tool,
        Sword,
        Staff,
        Bow
    }

    public enum ActionType
    {
        None,
        Attack,
        Shoot,
        Spell,
        Dig,
        Defend,
        Craftable
    }

    [Serializable]
    public class ComboAnimations
    {
        [SerializeField] public Sprite[] animations;
    }
}
