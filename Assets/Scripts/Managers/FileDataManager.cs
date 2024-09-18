using Assets.Scripts.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class FileDataManager
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";

        private const string KEY = "0rzSMntyDSSeSkhIEywoKalDqVNExq6hMqFqiQgXL8E=";
        private const string IV = "EGPnmiTEgQ3XdOYSyTUQnA==";
        private readonly string backupExtension = ".bak";


        public FileDataManager(string dataDirPath, string dataFilePath)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFilePath;
        }

        public GameData Load(string profileId, bool allowRestoreFromBackup = true)
        {
            Debug.Log("Entered Load");

            if (profileId == null)
            {
                return null;
            }

            string filePath = Path.Combine(_dataDirPath, profileId, _dataFileName);

            GameData loadedData = null;

            if (File.Exists(filePath))
            {
                try
                {
                    loadedData = ReadEncryptedData(filePath);
                }
                catch (Exception ex)
                {
                    if (allowRestoreFromBackup)
                    {
                        Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + ex);

                        bool rollbackSuccess = AttemptRollback(filePath);

                        if (rollbackSuccess)
                        {
                            loadedData = Load(profileId, false);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Error occured when trying to load file at path: {filePath} and backup did not work.\n {ex}");
                    }
                }
            }

            Debug.Log("Successfully loaded data");

            return loadedData;
        }

        public void Save(GameData data, string profileId)
        {
            if (profileId == null)
            {
                return;
            }

            string filePath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            string backupFilePath = filePath + backupExtension;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    WriteEncryptedData(data, stream);
                }

                GameData verifiedGameData = Load(profileId);

                if (verifiedGameData != null)
                {
                    File.Copy(filePath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be verified and backup could not be created.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured while saving data to file: {_dataFileName}, error: {e}");
            }
        }

        public void Delete(string profileId)
        {
            if (profileId == null)
            {
                return;
            }

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);

            try
            {
                if (File.Exists(fullPath))
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else
                {
                    Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete profile data for profileId: {profileId} at path: {fullPath} with error: {e}");
            }
        }

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            Dictionary<string, GameData> profileDictionary = new();

            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();

            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                string profileId = dirInfo.Name;

                string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);

                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning($"Skipping directory when loading all profiles because it does not contain data: {profileId}");
                    continue;
                }

                GameData profileData = Load(profileId);

                if (profileData != null)
                {
                    profileDictionary.Add(profileId, profileData);
                }
                else
                {
                    Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
                }
            }

            return profileDictionary;
        }

        private bool AttemptRollback(string fullPath)
        {
            bool success = false;
            string backupFilePath = fullPath + backupExtension;

            try
            {
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                }
                else
                {
                    throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to roll back to backup file at:{ backupFilePath} with error: {e}");
            }

            return success;
        }

        private static void WriteEncryptedData(GameData gameData, FileStream fs)
        {
            //using Aes aesProvider = Aes.Create();

            //aesProvider.Key = Convert.FromBase64String(KEY);
            //aesProvider.IV = Convert.FromBase64String(IV);

            //using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
            //using CryptoStream cryptoStream = new CryptoStream(fs, cryptoTransform, CryptoStreamMode.Write);

            //cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(gameData, Formatting.Indented)));

            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.Converters.Add(new PlayerDataConverter());

            fs.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(gameData, settings)));
        }

        private GameData ReadEncryptedData(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            //using Aes aesProvider = Aes.Create();

            //aesProvider.Key = Convert.FromBase64String(KEY);
            //aesProvider.IV = Convert.FromBase64String(IV);

            //using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            //    aesProvider.Key,
            //    aesProvider.IV
            //);

            using MemoryStream decryptionStream = new(fileBytes);

            //using CryptoStream cryptoStream = new CryptoStream(
            //    decryptionStream,
            //    cryptoTransform,
            //    CryptoStreamMode.Read
            //);

            //using StreamReader reader = new(cryptoStream);
            using StreamReader reader = new(decryptionStream);


            string result = reader.ReadToEnd();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new PlayerDataConverter());

            return JsonConvert.DeserializeObject<GameData>(result, settings);
        }
    }
}
