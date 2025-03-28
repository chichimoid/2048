using System.Collections.Generic;
using Game.Field;
using GlobalData;
using UnityEngine;

namespace Game
{
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
        }

        public void StartGame()
        {
            SaveManager.Instance.InitSave();
            InitScores();
            if (GameField.Instance.Cells.Count == 0) UpdateTurn();
        }

        private void InitScores()
        {
            _highScore = GameStateStaticContainer.GameState.highScore;
            OnHighScoreChanged?.Invoke(_highScore);
        
            _score = GameField.Instance.CalculateScore();
            OnScoreChanged?.Invoke(_score);
        }
    
        public void UpdateTurn()
        {
            GameField.Instance.CreateCell();
        
            _score = GameField.Instance.CalculateScore();
            OnScoreChanged?.Invoke(_score);
        
            if (_score > _highScore)
            {
                _highScore = _score;
                OnHighScoreChanged?.Invoke(_highScore);
            }
        
            if (GameField.Instance.CheckFieldFull())
            {
                EndGame();
            }
        }
    
        public void StopGame()
        {
            if (GameField.Instance.CheckFieldFull()) return;
        
            OnGameStopped?.Invoke(new GameState(GameField.Instance.Width, GameField.Instance.Height, GameField.Instance.Cells, _highScore));
        }

        public void EndGame()
        {
            OnGameStopped?.Invoke(new GameState(GameField.Instance.Width, GameField.Instance.Height, new List<Cell>(), _highScore));
            OnGameEnded?.Invoke();
        
            if (_score == _highScore)
            {
                OnNewHighScoreGameOver?.Invoke();
            }
        }
    
        public delegate void OnScoreChangedDelegate(int score);
        public event OnScoreChangedDelegate OnScoreChanged;
    
        public delegate void OnHighScoreChangedDelegate(int highScore);
        public event OnHighScoreChangedDelegate OnHighScoreChanged;
    
        public delegate void OnNewHighScoreGameOverDelegate();
        public event OnNewHighScoreGameOverDelegate OnNewHighScoreGameOver;
    
        public delegate void OnGameStoppedDelegate(GameState state);
        public event OnGameStoppedDelegate OnGameStopped;
    
        public delegate void OnGameEndedDelegate();
        public event OnGameEndedDelegate OnGameEnded;
    }
}