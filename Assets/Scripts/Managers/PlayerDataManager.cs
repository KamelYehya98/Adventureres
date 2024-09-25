using Assets.Scripts.Classes;
using Assets.Scripts.Data;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class PlayerDataManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField]
        public PlayerCameraManager cameraManager;

        [SerializeField]
        public DataPersistenceManager dataPersistenceManager;

        public GameObject playerPrefab;

        private List<GameObject> playerInstances = new();
        private PlayerControlScheme playerControlScheme = new();
        private Camera currentActiveCamera;

        private GameData gameData;

        private class PlayerControlScheme
        {
            public string Name { get; set; }
            public bool IsAvailable { get; set; }
            public bool IsMain { get; set; }
        }

        private static PlayerDataManager _instance;
        public static PlayerDataManager Instance
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
            _instance = FindObjectOfType<PlayerDataManager>();

            if (_instance == null)
            {
                GameObject playerDataObj = new GameObject("PlayerDataManager");
                _instance = playerDataObj.AddComponent<PlayerDataManager>();
                DontDestroyOnLoad(playerDataObj);
            }
        }

        private void Start()
        {
            playerControlScheme = new PlayerControlScheme()
            {
                Name = "Player2ControlScheme",
                IsAvailable = true,
                IsMain = true,
            };
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

        private void OnDestroy()
        {
            if (_instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name != "MainMenu")
            {
                InstantiatePlayer(false);

                DataPersistenceManager.Instance.LoadGame();
            }
        }

        public bool InstantiatePlayer(bool isNew = false)
        {
            if(!isNew && gameData == null)
            {
                Debug.LogError("Failed to instantiate player, gamedata object is null");

                return false;
            }

            Vector3 startPosition = new(playerInstances.Count * 2 + 2, 0, 0);

            GameObject player = Instantiate(playerPrefab, startPosition, Quaternion.identity);

            player.GetComponent<PlayerController>().Initialize(gameData.PlayerData);

            if (player.TryGetComponent(out PlayerInputController playerInputController))
            {
                playerInputController.AssignControlScheme(playerControlScheme.Name);
            }

            Camera mainCam = player.GetComponentInChildren<Camera>(true);
            CinemachineVirtualCamera vcam = player.GetComponentInChildren<CinemachineVirtualCamera>(true);

            if (mainCam == null)
            {
                Debug.LogError("Player's Camera is null");
                return false;
            }
            if (vcam == null)
            {
                Debug.LogError("Player's CinemachineVirtualCamera is null");
                return false;
            }

            vcam.Follow = player.transform;  // Ensure the virtual camera follows the player
            vcam.LookAt = player.transform;  // Optional: Ensure the virtual camera looks at the player

            if (currentActiveCamera != null)
            {
                currentActiveCamera.GetComponent<AudioListener>().enabled = false;
            }

            mainCam.enabled = true;
            mainCam.GetComponent<AudioListener>().enabled = true;
            currentActiveCamera = mainCam;

            cameraManager.AddPlayerCamera(vcam, mainCam);

            Canvas canvas = player.GetComponentInChildren<Canvas>(true);

            if (canvas == null)
            {
                Debug.LogError("Player's Canvas is null");
                return false;
            }

            canvas.worldCamera = mainCam;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingLayerName = "Background";

            playerInstances.Add(player);

            return true;
        }

        public bool AddNewPlayer(string playerName)
        {
            gameData = new GameData();

            PlayerData playerData = new(playerName);

            gameData.PlayerData = playerData;

            dataPersistenceManager.AddPlayer(playerData);

            return true;
        }

        public void LoadData(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void SaveData(GameData gameData)
        {
        }
    }
}