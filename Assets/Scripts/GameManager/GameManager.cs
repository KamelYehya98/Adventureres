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

    public GameData GameData => _gameData;

    #endregion

    #region Singleton Setup

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

    private static void SetupInstance()
    {
        _instance = FindObjectOfType<GameManager>();
        if (_instance == null)
        {
            GameObject gameObj = new GameObject("GameManager");
            _instance = gameObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    #endregion

    #region Scene Loading

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            InstantiateClass();
        }
    }

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
            // Add other cases for different classes
            default:
                Debug.LogError("Invalid class index selected.");
                return;
        }
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void InstantiateClass()
    {
        if (_selectedClassData != null)
        {
            GameObject player = Instantiate(_selectedClassData.prefab, Vector3.zero, Quaternion.identity);
            PlayerClass playerClass = player.GetComponent<PlayerClass>();
            playerClass.Initialize(_selectedClassData);
            
            player.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Selected class data is null.");
        }
    }

    #endregion
}
