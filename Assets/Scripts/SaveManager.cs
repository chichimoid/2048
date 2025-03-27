using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct GameState
{
    public GameState(int width, int height, List<Cell> cells, int highScore)
    {
        this.width = width;
        this.height = height;
        Cells = cells;
        this.highScore = highScore;
    }

    public int width;
    public int height;
    public List<Cell> Cells;
    public int highScore;
}

public class SaveManager : MonoBehaviour
{
    [Header("Configure")]
    [SerializeField] private string localSavePath;
    
    public static SaveManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void InitSave()
    {
        GameManager.Instance.OnGameEnded += StopSave;
        GameManager.Instance.OnGameStopped += StopSave;
    }

    public void StopSave(GameState state)
    {
        SaveGameState(state);
        
        GameManager.Instance.OnGameStopped -= SaveGameState;
        GameManager.Instance.OnGameEnded -= SaveGameState;
    }
    
    public void SaveGameState(GameState state)
    {
        string json = JsonUtility.ToJson(state);
        string path = Path.Combine(Application.persistentDataPath, localSavePath);
        File.WriteAllText(path, json);
    }

    public bool TryLoadData(out GameState state)
    {
        string path = Path.Combine(Application.persistentDataPath, localSavePath);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            state = JsonUtility.FromJson<GameState>(json);
            if (state.Cells.Count != 0)
            {
                return true;
            }
        }
        state = default;
        return false;
    }
}