using Assets.Scripts.Classes;
using Assets.Scripts.ScriptableObjects;
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

        public GameObject playerPrefab;
        private List<PlayerControls> playerControlsList = new();
        private List<GameObject> playerInstances = new();
        private List<PlayerControlScheme> playerControlSchemesList = new();
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

                Vector3 startPosition = new Vector3(playerInstances.Count * 2, 0, 0);

                GameObject player = Instantiate(playerPrefab, startPosition, Quaternion.identity);

                if (player.TryGetComponent(out PlayerInputController playerInputController))
                {
                    playerInputController.AssignControlScheme(controlScheme);
                    playerControlsList.Add(playerInputController.inputActions);
                }

                PlayerClassManager playerClassManager = player.GetComponent<PlayerClassManager>();

                // Alternate classes for new players for demonstration
                PlayerClassData classData = (playerInstances.Count % 2 == 0) ? classManager.mageData : classManager.warriorData;

                playerInstances.Add(player);
            }
        }

        #endregion
    }
}