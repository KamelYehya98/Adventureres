using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Only Gameplay")]
        public ItemType type;
        public ActionType actionType;
        public Vector2Int range = new(5, 4);


        [Header("Only UI")]
        public bool stackable = true;

        [Header("Both")]
        public Sprite image;
    }

    public enum ItemType
    {
        Loot,
        Tool
    }

    public enum ActionType
    {
        Attack,
        Dig,
        Defend,
        Craftable
    }
}
