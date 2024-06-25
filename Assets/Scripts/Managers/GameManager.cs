using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Game Settings")]
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameData defaultData;

    private ClassManager classManager;
    private Transform spawnPoint;

    private PlayerClassData selectedClassData;

    public GameData GameData
    {
        get
        {
            return gameData;
        }
    }
    public ClassManager ClassManager
    {
        get
        {
            return classManager;
        }
    }
    public Transform SpawnPoint
    {
        get
        {
            return spawnPoint;
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

    #region Utility Methods
    public void SelectClass(int classIndex)
    {
        switch (classIndex)
        {
            case 0:
                selectedClassData = classManager.mageData;
                break;
            case 1:
                selectedClassData = classManager.warriorData;
                break;
            case 2:
                selectedClassData = classManager.marksmanData;
                break;
        }
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            InstantiateClass();
        }
    }

    private void InstantiateClass()
    {
        GameObject player = new GameObject(selectedClassData.className);
        PlayerClass playerClass = player.AddComponent(System.Type.GetType(selectedClassData.className)) as PlayerClass;

        playerClass.Initialize(selectedClassData);
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
    }
    #endregion
}
