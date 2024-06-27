using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header ("Player Settings")]
    [SerializeField] public float playerMovementSpeed = 1f;

    [Header("Enemy Settings")]
    [SerializeField] public float enemyMovementSpeed = 1f;

    [Header("Game Settings")]
    [SerializeField] public Transform spawnPoint;
}
