using Assets.Scripts.Data;
using Assets.Scripts.Player;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour, IDataPersistence
    {
        public GameCameraManager cameraManager;

        public GameObject playerPrefab;

        private List<GameObject> _playerInstances = new();
        private PlayerControlScheme _playerControlScheme = new();
        private Camera _currentActiveCamera;

        private GameData _gameData;

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

        public void Awake()
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

        public void Start()
        {
            _playerControlScheme = new PlayerControlScheme()
            {
                Name = "Player2ControlScheme",
                IsAvailable = true,
                IsMain = true,
            };
        }

        public void OnDestroy()
        {
            if (_instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
            {
                GameObject playerDataObj = new("PlayerDataManager");
                _instance = playerDataObj.AddComponent<GameManager>();
                DontDestroyOnLoad(playerDataObj);
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "MainMenu")
            {
                InstantiatePlayerObject();

                DataPersistenceManager.Instance.LoadGame();
            }
        }

        private GameObject InstantiatePlayerWithPosition()
        {
            Vector3 startPosition = new(_playerInstances.Count * 2 + 2, 0, 0);

            return Instantiate(playerPrefab, startPosition, Quaternion.identity);
        }

        public bool InstantiatePlayerObject()
        {
            if(_gameData == null)
            {
                Debug.LogError("Failed to instantiate player, gamedata object is null");

                return false;
            }

            GameObject player = InstantiatePlayerWithPosition();

            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();

                if (playerController != null)
                {
                    playerController.InstantiatePlayer(_gameData.PlayerData, _playerControlScheme.Name);

                    _playerInstances.Add(player);

                    if(_currentActiveCamera == null)
                    {
                        _currentActiveCamera = playerController.coreController.camera;
                    }
                    else
                    {
                        playerController.coreController.camera.GetComponent<AudioListener>().enabled = false;
                    }

                    cameraManager.AddPlayerCamera(playerController.coreController.cinemachine, playerController.coreController.camera);

                    return true;
                }
            }

            return false;
        }

        public void AddNewPlayerToGameData(string playerName)
        {
            _gameData = new GameData();

            PlayerData playerData = new(playerName);

            _gameData.PlayerData = playerData;
        }

        public void LoadData(GameData gameData)
        {
            this._gameData = gameData;
        }

        public void SaveData(GameData gameData)
        {
        }

        public void SetGameData(GameData gameData)
        {
            _gameData = gameData;
        }

        public GameData GetGameData()
        {
            return _gameData;
        }
    }

    public class PlayerControlScheme
    {
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsMain { get; set; }
    }

}