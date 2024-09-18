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
        [SerializeField] private string fileName;

        [Header("Auto Saving Configuration")]
        [SerializeField] private float autoSaveTimeSeconds = 10f;

        private string selectedProfileId = "";

        private Coroutine autoSaveCoroutine;

        public static DataPersistenceManager Instance { get; private set; }

        private List<IDataPersistence> _persistenceList = new();
        private FileDataManager _fileDataManager;
        private GameData _gameData;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            _fileDataManager = new FileDataManager(Application.persistentDataPath, fileName);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _persistenceList = FindAllDataPersistenceObjects();

            // start up the auto saving coroutine
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }

            autoSaveCoroutine = StartCoroutine(AutoSave());
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            // update the profile to use for saving and loading
            this.selectedProfileId = newProfileId;
        }


        public void DeleteProfileData(string profileId)
        {
            // delete the data for this profile id
            _fileDataManager.Delete(profileId);
        }

        public void NewGame()
        {
            _gameData = new GameData()
            {
                Id = Guid.NewGuid(),
                PlayerData = new("Default Player")
            };

            selectedProfileId = _gameData.Id.ToString();
        }

        private void Start()
        {
            _fileDataManager = new FileDataManager(Application.persistentDataPath, fileName);
            _persistenceList = FindAllDataPersistenceObjects();
            if(_persistenceList == null || _persistenceList.Count == 0)
            {
                Debug.LogError("Couldnt find any persistent objects");
            }
        }

        public void LoadGame()
        {
            _persistenceList = FindAllDataPersistenceObjects();

            _gameData = _fileDataManager.Load(selectedProfileId);

            if(_gameData == null)
            {
                Debug.LogError("Failed to laod game, creating a new one....");
                NewGame();
            }

            foreach (IDataPersistence dataPersistence in _persistenceList)
            {
                dataPersistence.LoadData(_gameData);

                Debug.Log("Loaded game for dataPersistence object of type: " + dataPersistence.GetType().FullName);
            }

            Debug.Log("Loaded game for player " + _gameData.PlayerData.PlayerName);
        }

        public void SaveGame()
        {
            if (_gameData == null)
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            foreach (IDataPersistence dataPersistence in _persistenceList)
            {
                dataPersistence.SaveData(_gameData);
            }

            _gameData.LastUpdated = DateTime.Now.ToBinary();

            _fileDataManager.Save(_gameData, selectedProfileId);
        }

        public bool HasGameData()
        {
            return _gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return _fileDataManager.LoadAllProfiles();
        }

        public void AddPlayer(PlayerData playerData)
        {
            if(_gameData == null)
            {
                NewGame();
            }

            _gameData.PlayerData = playerData;

            SaveGame();
        }
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoSaveTimeSeconds);
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
