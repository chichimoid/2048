using System;
using System.IO;
using Game;
using UnityEngine;

namespace GlobalData
{
    public class SaveManager : MonoBehaviour
    {
        [Header("Configure")]
        [SerializeField] private string saveFileName = "save";
    
        public static SaveManager Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void InitSave()
        {
            GameManager.Instance.OnGameStopped += StopSave;
        }

        public void StopSave(GameState state)
        {
            SaveGameState(state);
        
            GameManager.Instance.OnGameStopped -= SaveGameState;
        }
    
        public void SaveGameState(GameState state)
        {
            string json = JsonUtility.ToJson(state);
            string path = FieldSizeToPath(state.width, state.height);
            File.WriteAllText(path, json);
        }

        public bool TryLoadGameState(int width, int height, out GameState state)
        {
            string path = FieldSizeToPath(width, height);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                state = JsonUtility.FromJson<GameState>(json);
                return true;
            }
            state = default;
            return false;
        }

        private string FieldSizeToPath(int width, int height)
        {
            var path = $"{saveFileName}_{width.ToString()}x{height.ToString()}.json";
            return Path.Combine(Application.persistentDataPath, path);
        }
    }
}