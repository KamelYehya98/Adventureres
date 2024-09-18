using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerData
    {
        public Guid Id;
        public string PlayerName;
        public int Level;
        public float Experience; 
        public float Mana;
        public PlayerSkills Skills;
        public SerializableDictionary<InventoryItemData, int> Inventory;

        public PlayerData(string playerName) 
        { 
            Id = Guid.NewGuid();
            PlayerName = playerName;
            Level = 1;
            Experience = 0;
            Mana = 100;
            Skills = new();
            Inventory = new();
        }
    }

    [Serializable]
    public class PlayerSkills
    {
        public float Vitality;
        public float Regeneration;
        public float AttackPower;
        public float CriticalStrike;
        public float Defense;
        public float Agility;

        public PlayerSkills() 
        {
            Vitality = 100;
            Regeneration = 15;
            AttackPower = 15;
            CriticalStrike = 0.05f;
            Defense = 10;
            Agility = 6;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        public string Id;
        public int Index;

        public InventoryItemData(string id, int index)
        {
            Id = id;
            Index = index;
        }
    }
    // Vitality: Player HP and Max HP
    // Regeneration: Player HP and Mana regeneration rate
    // CriticalStrike: chance percentage to perform a strike that deals x2 the damage
    // Agility: Player's movement and attack speed
    // Defense: Player's ability to nullify taken damage
    // AttackPower: damage dealt by the player, abilities must scale with this value
}