using Assets.Scripts.Classes;
using Assets.Scripts.ScriptableObjects;
using Cinemachine;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        public PlayerCameraManager cameraManager;

        public GameObject playerPrefab;
        private List<PlayerControls> playerControlsList = new();
        private List<GameObject> playerInstances = new();
        private List<PlayerControlScheme> playerControlSchemesList = new();
        private Camera currentActiveCamera;

        public GameData GameData => _gameData;

        private class PlayerControlScheme
        {
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

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
            playerControlSchemesList = new()
            {
                new PlayerControlScheme()
                {
                    Name = "Player2ControlScheme",
                    IsActive = true
                },
                new PlayerControlScheme()
                {
                    Name = "Player1ControlScheme",
                    IsActive = true
                }
            };

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

        public bool HasAvailableControlScheme()
        {
            return playerControlSchemesList != null && playerControlSchemesList.Any(x => x.IsActive);
        }

        private string GetAvailableControlScheme()
        {
            return playerControlSchemesList?.FirstOrDefault(x => x.IsActive)?.Name;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "SampleScene")
            {
                //InstantiateClass();
            }
        }

        private void DeactivateControlScheme(string schemeName)
        {
            if (playerControlSchemesList != null && playerControlSchemesList.Any())
            {
                foreach (var scheme in playerControlSchemesList)
                {
                    if (scheme.Name == schemeName)
                    {
                        scheme.IsActive = false;
                        return;
                    }
                }
            }

            Debug.LogError("Control Scheme not found, failed to deactivate");
        }

        public void AddPlayer()
        {
            if (!HasAvailableControlScheme())
            {
                Debug.LogError("No control schemes available for a new player");
                return;
            }

            string controlScheme = GetAvailableControlScheme();

            if (!string.IsNullOrEmpty(controlScheme))
            {
                DeactivateControlScheme(controlScheme);

                Vector3 startPosition = new Vector3(playerInstances.Count * 2 + 10, 0, 0);

                GameObject player = Instantiate(playerPrefab, startPosition, Quaternion.identity);

                if (player.TryGetComponent(out PlayerInputController playerInputController))
                {
                    playerInputController.AssignControlScheme(controlScheme);
                    playerControlsList.Add(playerInputController.inputActions);
                }

                PlayerClassManager playerClassManager = player.GetComponent<PlayerClassManager>();

                // Alternate classes for new players for demonstration
                PlayerClassData classData = (playerInstances.Count % 2 == 0) ? classManager.mageData : classManager.warriorData;

                Camera mainCam = player.GetComponentInChildren<Camera>(true);
                CinemachineVirtualCamera vcam = player.GetComponentInChildren<CinemachineVirtualCamera>(true);

                if (mainCam == null)
                {
                    Debug.LogError("Player's Camera is null");
                    return;
                }
                if (vcam == null)
                {
                    Debug.LogError("Player's CinemachineVirtualCamera is null");
                    return;
                }

                vcam.Follow = playerClassManager.transform;  // Ensure the virtual camera follows the player
                vcam.LookAt = playerClassManager.transform;  // Optional: Ensure the virtual camera looks at the player

                if (currentActiveCamera != null)
                {
                    currentActiveCamera.GetComponent<AudioListener>().enabled = false;
                }

                mainCam.enabled = true;
                mainCam.GetComponent<AudioListener>().enabled = true;
                currentActiveCamera = mainCam;

                cameraManager.AddPlayerCamera(vcam, mainCam);

                Canvas canvas = player.GetComponentInChildren<Canvas>(true);

                if(canvas == null)
                {
                    Debug.LogError("Player's Canvas is null");
                    return;
                }

                canvas.worldCamera = mainCam;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.sortingLayerName = "Background";

                playerInstances.Add(player);
            }
        }

        #endregion
    }
}