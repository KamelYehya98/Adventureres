using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GlobalGameData
    {
        public int MaxPlayers = 2;
        public List<GameData> GameData;
    }
}