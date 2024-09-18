using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public Guid Id;
        public long LastUpdated;
        public PlayerData PlayerData;

        public GameData() 
        {
            Id = Guid.NewGuid();
            PlayerData = new("Defualt Player");
        }
    }
}