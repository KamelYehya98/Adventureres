using Assets.Scripts.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public static class GlobalGameManager
    {
        public static void CreateNewGame(GameData gameData)
        {
            if (gameData == null)
            {
                Debug.LogError("Failed to create new game");

                return;
            }

            string path = Path.Combine(Application.persistentDataPath, gameData.Id.ToString(), "data.json");

            File.Create(path);

            File.WriteAllText(path, JsonConvert.SerializeObject(gameData));
        }

        public static Dictionary<string, GameData> LoadExistingGames()
        {
            Dictionary<string, GameData> gameFiles = new();

            string path = Application.persistentDataPath;

            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(path).EnumerateDirectories();

            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                string gameId = dirInfo.Name;

                string fullPath = Path.Combine(path, gameId);

                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Skipping directory because it's not a save file directory, folder name: " + gameId);

                    continue;
                }

                //GameData gameData = SaveDataManager.LoadData(new Guid(gameId));

                //if (gameData == null)
                //{
                //    Debug.LogError("Failed to load data from game file with id: " + gameId);
                //}

                //gameFiles.Add(gameId, gameData);
            }

            return gameFiles;
        }
    }
}
