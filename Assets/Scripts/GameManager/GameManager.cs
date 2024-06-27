using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Game Settings")]
    [SerializeField]
    private GameData _gameData;

    public ClassManager classManager;

    private PlayerClassData _selectedClassData;

    public GameData GameData
    {
        get
        {
            return _gameData;
        }
    }
    #endregion

    #region Singleton Setup
    // To setup Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                SetupInstance();
            }

            return _instance;
        }
    }
    #endregion

    #region Method to Create Singleton Instance
    private static void SetupInstance()
    {
        _instance = FindObjectOfType<GameManager>();

        if (_instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            _instance = gameObj.AddComponent<GameManager>();
        }
    }
    #endregion

    #region Method to initiate selected class data
    public void SelectClass(int classIndex)
    {
        switch (classIndex)
        {
            case 0:
                _selectedClassData = classManager.mageData;
                break;
            case 1:
                _selectedClassData = classManager.warriorData;
                break;
        }

        LoadGameScene();
    }
    #endregion

    private void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            InstantiateClass();
        }
    }

    private void InstantiateClass()
    {
        if (_selectedClassData != null)
        {
            GameObject player = Instantiate(_selectedClassData.prefab, Vector3.zero, Quaternion.identity);
            PlayerClass playerClass = player.GetComponent<PlayerClass>();
            playerClass.Initialize(_selectedClassData);
            player.transform.SetPositionAndRotation(_gameData.spawnPoint.position, _gameData.spawnPoint.rotation);
        }
    }
}
