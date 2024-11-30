using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string _fileName;

        [Header("Auto Saving Configuration")]
        [SerializeField] private readonly float _autoSaveTimeSeconds = 10f;

        private string selectedProfileId = "";

        private Coroutine autoSaveCoroutine;

        public static DataPersistenceManager Instance { get; private set; }
        public GameManager gameManager;

        private List<IDataPersistence> _persistenceList = new();
        private FileDataManager _fileDataManager;
        
        public void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(this.gameObject);

            _fileDataManager = new FileDataManager(Application.persistentDataPath, _fileName);
        }

        public void Start()
        {
            _persistenceList = FindAllDataPersistenceObjects();

            gameManager = FindObjectOfType<GameManager>();

            if (_persistenceList == null || _persistenceList.Count == 0)
            {
                Debug.LogError("Couldnt find any persistent objects");
            }
        }

        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnApplicationQuit()
        {
            SaveGame();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _persistenceList = FindAllDataPersistenceObjects();

            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }

            autoSaveCoroutine = StartCoroutine(AutoSave());
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            this.selectedProfileId = newProfileId;
        }

        public void DeleteProfileData(string profileId)
        {
            _fileDataManager.Delete(profileId);
        }

        public void NewGame()
        {
            GameData gameData = new()
            {
                Id = Guid.NewGuid(),
                PlayerData = new("Default Player")
            };

            gameManager.SetGameData(gameData);

            selectedProfileId = gameData.Id.ToString();
        }

        public void LoadGame()
        {
            _persistenceList = FindAllDataPersistenceObjects();

            GameData gameData = _fileDataManager.Load(selectedProfileId);

            gameManager.SetGameData(gameData);

            if (gameData == null)
            {
                Debug.LogError("Failed to laod game, creating a new one....");
                NewGame();
            }

            foreach (IDataPersistence dataPersistence in _persistenceList)
            {
                dataPersistence.LoadData(gameData);

                Debug.Log("Loaded game for dataPersistence object of type: " + dataPersistence.GetType().FullName);
            }


            Debug.Log("Loaded game for player " + gameData.PlayerData.PlayerName);
        }

        public void SaveGame()
        {
            if(gameManager == null)
            {
                Debug.LogError("game manager is null");
            }

            GameData gameData = gameManager.GetGameData();

            if (gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            foreach (IDataPersistence dataPersistence in _persistenceList)
            {
                dataPersistence.SaveData(gameData);
            }

            gameData.LastUpdated = DateTime.Now.ToBinary();

            _fileDataManager.Save(gameData, selectedProfileId);
        }

        public bool HasGameData()
        {
            return gameManager.GetGameData() != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return _fileDataManager.LoadAllProfiles();
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(_autoSaveTimeSeconds);
                SaveGame();
                Debug.Log("Auto Saved Game");
            }
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(list);
        }
    }
}
