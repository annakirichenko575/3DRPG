using Infrastructure.Services;
using System;
using UnityEngine;

namespace SaveSystem
{
    public class SaverService : IService
    {
        private const string Save = "Save";

        private SaveData saveData;

        public Vector3 PlayerPosition => saveData.PlayerPosition;

        public void SetPlayerPosition(Vector3 playerPosition)
        {
            saveData.PlayerPosition = playerPosition;
        }

        public void SaveData()
        {
            string saveJson = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(Save, saveJson);
            PlayerPrefs.Save(); // Явное сохранение для надежности
        }

        public void Load()
        {
            string saveJson = PlayerPrefs.GetString(Save);

            if (String.IsNullOrEmpty(saveJson))
            {
                saveData = new SaveData();
            }
            else
            {
                saveData = JsonUtility.FromJson<SaveData>(saveJson);
            }

            // Автоматическая телепортация игрока после загрузки
            TeleportPlayerToSavedPosition();
        }

        private void TeleportPlayerToSavedPosition()
        {
            // Поиск игрока по тегу (более надежно, чем FindObjectOfType)
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = PlayerPosition;
                Debug.Log($"Player teleported to saved position: {PlayerPosition}");
            }
            else
            {
                Debug.LogWarning("Player object not found in scene!");
            }
        }
    }
}
