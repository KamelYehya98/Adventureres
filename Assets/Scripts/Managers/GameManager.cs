using Assets.Scripts.Classes;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [Header("Game Settings")]
        [SerializeField]
        private GameData _gameData;

        [SerializeField]
        public ClassManager classManager;

        public GameObject playerPrefab;
        private List<PlayerControls> playerControlsList = new();
        private List<GameObject> playerInstances = new();

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

        private void Start()
        {
            AddPlayer();
            AddPlayer();
        }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && playerInstances.Count < 4)
            {
                AddPlayer();
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
                //InstantiateClass();
            }
        }

        public void AddPlayer()
        {
            Vector3 startPosition = new Vector3(playerInstances.Count * 2, 0, 0);

            GameObject player = Instantiate(playerPrefab, startPosition, Quaternion.identity);

            if(player.TryGetComponent(out PlayerInputController playerInputController))
            {
                playerControlsList.Add(playerInputController.inputActions);
            }

            PlayerClassManager playerClassManager = player.GetComponent<PlayerClassManager>();

            // Alternate classes for new players for demonstration
            PlayerClassData classData = (playerInstances.Count % 2 == 0) ? classManager.mageData : classManager.warriorData;

            playerInstances.Add(player);
        }

        //public void SelectClass(int classIndex)
        //{
        //    switch (classIndex)
        //    {
        //        case 0:
        //            _selectedClassData = classManager.mageData;
        //            break;
        //        case 1:
        //            _selectedClassData = classManager.warriorData;
        //            break;
        //        // Add other cases for different classes
        //        default:
        //            //Debug.LogError("Invalid class index selected.");
        //            return;
        //    }
        //    LoadGameScene();
        //}

        //private void LoadGameScene()
        //{
        //    SceneManager.LoadScene("SampleScene");
        //}



        #endregion
    }
}
