using System;
using System.Collections.Generic;
using System.Linq;
using GlobalData;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Field
{
    public class GameField : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject emptyCellPrefab;
        [SerializeField] private GameObject valueCellPrefab;
    
        private RectTransform _gameFieldTransform;
    
        private int _width;
        private int _height;
        private float _cellDistance = 64;
        private float _cellScaleMult = 1;
        private float _xOffset;
        private float _yOffset;
    
        public int Width => _width;
        public int Height => _height;
        public float CellDistance => _cellDistance;
        public float CellScaleMult => _cellScaleMult;
        public float XOffset => _xOffset;
        public float YOffset => _yOffset;
        public List<Cell> Cells { get; private set; } // Personally, wouldn't use a list, but that was the requirement.
        // All the simple algorithms suddenly become much slower due to indexing now taking O(N) instead of O(1).
        // Time complexity still negligible though, since the field is small.
    
        public static GameField Instance { get; private set; }
    
        private void Awake()
        {
            Instance = this;
        
            _width = GameStateStaticContainer.GameState.width;
            _height = GameStateStaticContainer.GameState.height;
        
            _xOffset = (float)-(_width - 1) / 2;
            _yOffset = (float)-(_height - 1) / 2;
        }
    
        private void Start()
        {
            _gameFieldTransform = GetComponent<RectTransform>();

            GameFieldInputHandler.Instance.OnMoveCellsInput += MoveCells;
        
            if (_width != 4 || _height != 4)
            {
                AdjustSizes();
            }
        
            SpawnEmptyCells();
        
            LoadCells(GameStateStaticContainer.GameState.cells ?? new List<Cell>());
        
            OnFieldSpawned?.Invoke();
        }
    
        /// <summary>
        /// Adjusts cell and field sizes along with cell distance for different width and height.
        /// </summary>
        private void AdjustSizes()
        {
            _cellScaleMult = 4 / (float)Math.Max( _width, _height);
            _cellDistance *= _cellScaleMult;
        
            float fieldWidthMult = (float)_width / 4 * _cellScaleMult;
            float newDeltaX = _gameFieldTransform.sizeDelta.x * fieldWidthMult;
            float fieldHeightMult = (float)_height / 4 * _cellScaleMult;
            float newDeltaY = _gameFieldTransform.sizeDelta.y * fieldHeightMult;
            _gameFieldTransform.sizeDelta = new Vector2(newDeltaX, newDeltaY);
        }
    
        private void SpawnEmptyCells()
        {
            for (int x = 0; x < _width; x++)
            {                                                                            
                for (int y = 0; y < _height; y++)
                {
                    var cell =  Instantiate(emptyCellPrefab.transform, transform);
                    var cellRectTransform = cell.GetComponent<RectTransform>();
                    cellRectTransform.anchoredPosition = new Vector2((_xOffset + x) * _cellDistance, (_yOffset + y) * _cellDistance);

                    if (_width != 4 || _height != 4)
                    {
                        cellRectTransform.sizeDelta *= new Vector2(_cellScaleMult, _cellScaleMult);
                    }
                }
            }
        }
    
        [CanBeNull]
        private Cell GetCellFromPosition(Vector2Int position)
        {
            foreach (var cell in Cells)
            {
                if (cell.Position == position)
                {
                    return cell;
                }
            }

            return null;
        }
    
        private bool IsEmpty(Vector2Int position)
        {
            return GetCellFromPosition(position) == null;
        }
    
        private bool IsMergeable(int value, Vector2Int position)
        {
            var cell = GetCellFromPosition(position);
            return cell != null && cell.Value == value;
        }
    
        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.y < 0 || position.x >= _width || position.y >= _height;
        }
    
        public Vector2Int GetEmptyPosition()
        {
            // Nondeterministic (Las Vegas) algo. On average, takes O(N^2).
            // Basically a Bernoulli distributed time, for bigger N gets closer to O(N^2).
            // A deterministic algo (if we're only using the Cells list) would also take around O(N^2).
            // That being said, considering N <= 7 for our game, this time complexity is negligible.
            int rndX, rndY;
            do
            {
                rndX = Random.Range(0, _width);
                rndY = Random.Range(0, _height);
            } while (!IsEmpty(new Vector2Int(rndX, rndY)));
        
            return new Vector2Int(rndX, rndY);
        }

        public void CreateCell()
        {
            int valueDistr = Random.Range(1, 6); // Hoping that Unity.Random gives a uniform distribution.
            int value = valueDistr == 1 ? 2 : 1;
        
            var pos = GetEmptyPosition();
        
            var cell = new Cell(pos, value);

            Cells.Add(cell);
        
            CreateCellObject(cell);
        }

        public void LoadCells(List<Cell> cells)
        {
            Cells = cells;
            foreach (var cell in Cells)
            {
                CreateCellObject(cell);
            }
        }

        private void CreateCellObject(Cell cell)
        {
            var cellObject = Instantiate(valueCellPrefab.transform, transform);
        
            var cellView = cellObject.GetComponent<CellView>();
            cellView.Init(cell);
        }
    
        public void MoveCells(Direction direction)
        {
            var traversalOrder = CreateTraversalOrder(direction);

            bool movedAnything = false;
            foreach (var pos in traversalOrder)
            {
                var cell = GetCellFromPosition(pos);
                if (cell == null) continue;

                MoveCell(cell, direction, out bool moved);
                movedAnything = movedAnything || moved;
            }
        
            if (movedAnything) OnCellsMoved?.Invoke();
        }

        private List<Vector2Int> CreateTraversalOrder(Direction direction)
        {
            var order = new List<Vector2Int>();
            switch (direction)
            {
                case Direction.Up:
                    for (int y = _height - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < _width; x++)
                        {
                            order.Add(new Vector2Int(x, y));
                        }
                    }

                    break;
                case Direction.Down:
                    for (int y = 0; y < _height; y++)
                    {
                        for (int x = 0; x < _width; x++)
                        {
                            order.Add(new Vector2Int(x, y));
                        }
                    }

                    break;
                case Direction.Right:
                    for (int x = _width - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            order.Add(new Vector2Int(x, y));
                        }
                    }

                    break;
                case Direction.Left:
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            order.Add(new Vector2Int(x, y));
                        }
                    }

                    break;
            }
            return order;
        }

        private void MoveCell(Cell cell, Direction direction, out bool moved)
        {
            var newPos = cell.Position;
            var directionVector = direction.ToVector2Int();
            while (!IsOutOfBounds(newPos + directionVector))
            {
                if (IsEmpty(newPos + directionVector))
                {
                    newPos += directionVector;
                }
                else if (IsMergeable(cell.Value, newPos + directionVector))
                {
                    Merge(cell, GetCellFromPosition(newPos + directionVector));
                    moved = true;
                    return;
                }
                else
                {
                    break;
                }
            }
            if (cell.Position == newPos)
            {
                moved = false;
                return;
            }

            moved = true;
            cell.Position = newPos;
        }
    
        private void Merge(Cell incomingCell, Cell overriddenCell)
        {
            incomingCell.Position = overriddenCell.Position;
            ++incomingCell.Value;
            Cells.Remove(overriddenCell);
            overriddenCell.Destroy();
        }

        public int CalculateScore()
        {
            return Cells.Sum(cell => (int)Math.Pow(2, cell.Value));
        }

        public bool CheckFieldFull()
        {
            if (Cells.Count < _width * _height) return false;
        
            var upVector = Direction.Up.ToVector2Int();
            var downVector = Direction.Down.ToVector2Int();
            var rightVector = Direction.Right.ToVector2Int();
            var leftVector = Direction.Left.ToVector2Int();

            foreach (var cell in Cells)
            {
                int value = cell.Value;
                Vector2Int pos = cell.Position;
                if (IsMergeable(value, pos + upVector) || IsMergeable(value, pos + downVector) ||
                    IsMergeable(value, pos + rightVector) || IsMergeable(value, pos + leftVector))
                {
                    return false;
                }
            }
            return true;
        }

        public delegate void OnFieldSpawnedDelegate();
        public event OnFieldSpawnedDelegate OnFieldSpawned;
    
        public delegate void OnCellsMovedDelegate();
        public event OnCellsMovedDelegate OnCellsMoved;
    }
}