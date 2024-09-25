﻿using UnityEngine;

namespace Assets.Scripts.Player
{
    public enum TeamIndex : sbyte
    {
        None = -1,
        Neutral = 0,
        Player,
        Enemy,
        Count
    }

    public class TeamComponent : MonoBehaviour
    {
        [SerializeField] private TeamIndex _teamIndex = TeamIndex.None;
        public TeamIndex teamIndex
        {
            set
            {
                if (_teamIndex == value)
                {
                    return;
                }

                _teamIndex = value;


            }
            get { return _teamIndex; }
        }
    }
}
