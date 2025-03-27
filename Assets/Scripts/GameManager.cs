using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _score;
    private int _highScore;
    
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameField.Instance.OnFieldSpawned += StartGame;
        GameField.Instance.OnCellsMoved += UpdateTurn;
        GameField.Instance.OnFieldFull += EndGame;
    }

    public void StartGame()
    {
        SaveManager.Instance.InitSave();
        
        UpdateTurn();
    }
    
    public void UpdateTurn()
    {
        GameField.Instance.CreateCell();
        _score = GameField.Instance.CalculateScore();
        OnScoreChanged?.Invoke(_score);
    }

    public void StopGame()
    {
        OnGameStopped?.Invoke(new GameState(GameField.Instance.Width, GameField.Instance.Height, GameField.Instance.Cells, _score > _highScore ? _score : _highScore));
    }

    public void EndGame()
    {
        OnGameEnded?.Invoke(new GameState(0, 0, null, _highScore));
        Debug.Log("Game Over");
    }
    
    public delegate void OnScoreChangedDelegate(int score);
    public event OnScoreChangedDelegate OnScoreChanged;
    
    public delegate void OnGameStoppedDelegate(GameState state);
    public event OnGameStoppedDelegate OnGameStopped;
    
    public delegate void OnGameEndedDelegate(GameState state);
    public event OnGameEndedDelegate OnGameEnded;
}