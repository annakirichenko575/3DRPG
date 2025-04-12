using Infrastructure;
using Infrastructure.Services;
using System;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerRepository : IService
    {
        public const int MaxHealth = 100;
        public const float MaxMana = 100f;

        private const string Save = "PlayerSave";

        private PlayerSaveData saveData;

        public Vector3 Position => saveData.Position;
        public int Health => saveData.Health;
        public float Mana => saveData.Mana;

        public event Action OnManaChanged;

        public PlayerRepository()
        {
            saveData = new PlayerSaveData();
            ResetData();
        }

        public void SetPlayerPosition(Vector3 playerPosition)
        {
            saveData.Position = playerPosition;
        }

        public void SetPlayerHealth(int value)
        {
            saveData.Health = value;
        }

        public void SetPlayerMana(float value)
        {
            saveData.Mana = Mathf.Clamp(value, 0, MaxMana);
            OnManaChanged?.Invoke(); 
            SaveData();
        }

        public void SaveData()
        {
            string saveJson = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(Save, saveJson);
            PlayerPrefs.Save(); 
        }

        public void LoadData()
        {
            string saveJson = PlayerPrefs.GetString(Save);

            if (String.IsNullOrEmpty(saveJson))
            {
                ResetData();
            }
            else
            {
                saveData = JsonUtility.FromJson<PlayerSaveData>(saveJson);
            }
        }

        public void ResetData()
        {
            saveData.Health = MaxHealth;
            saveData.Mana = MaxMana;
        }
    }
}
