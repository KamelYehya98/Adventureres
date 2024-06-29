using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header ("Player Settings")]
    [SerializeField] public float playerMovementSpeed = 4f;

    [Header("Enemy Settings")]
    [SerializeField] public float enemyMovementSpeed = 4f;
}
