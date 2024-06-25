using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Game Settings")]
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameData defaultData;

    public GameData GameData
    {
        get
        {
            return gameData;
        }
    }
    #endregion

    #region Singleton Setup
    // To setup Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }
    #endregion
    #region Method to Create Singleton Instance
    private static void SetupInstance()
    {
        instance = FindObjectOfType<GameManager>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
        }
    }
    #endregion
}
