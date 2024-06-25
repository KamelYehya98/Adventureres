using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ClassManager classManager;
    public Transform spawnPoint;

    private PlayerClassData selectedClassData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
}
