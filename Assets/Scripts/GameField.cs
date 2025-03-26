using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
    [SerializeField] private GameObject emptyCellPrefab;
    [SerializeField] private GameObject valueCellPrefab;
    
    private GameField _gameField;
    private RectTransform _gameFieldTransform;

    private int _width;
    private int _height;
    private float _cellDistance = 64;
    private float _cellScaleMult = 1;
    private float _xOffset;
    private float _yOffset;
    
    private readonly List<Vector2Int> _vacant = new();
    
    public int Width => _width;
    public int Height => _height;
    public float CellDistance => _cellDistance;
    public float CellScaleMult => _cellScaleMult;
    public float XOffset => _xOffset;
    public float YOffset => _yOffset;
    public List<Cell> Cells { get; } = new();
    public static GameField Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _width = GameFieldDataTransmitter.Instance.Width;
        _height = GameFieldDataTransmitter.Instance.Height;
        
        _xOffset = (float)-(_width - 1) / 2;
        _yOffset = (float)-(_height - 1) / 2;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _vacant.Add(new Vector2Int(x, y));
            }
        }
    }
    
    private void Start()
    {
        _gameField = GetComponent<GameField>();
        _gameFieldTransform = GetComponent<RectTransform>();
        _width = _gameField.Width;
        _height = _gameField.Height;

        if (_width != 4 || _height != 4)
        {
            AdjustSizes();
        }
        SpawnEmptyCells();
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
    
    public Vector2Int GetEmptyPosition()
    {
        int rndIndex = Random.Range(0, _vacant.Count);
        return _vacant[rndIndex];
    }

    public void CreateCell()
    {
        if (_vacant.Count == 0)
        {
            Debug.Log("Game field is full");
            return;
        }
        
        int valueDistr = Random.Range(1, 11); // Hoping that Unity.Random gives a uniform distribution.
        int value = valueDistr == 1 ? 2 : 1;
        
        var pos = GetEmptyPosition();
        
        var cell = new Cell(pos, value);
        
        _vacant.Remove(pos);
        Cells.Add(cell);
        
        var cellObject = Instantiate(valueCellPrefab.transform, transform);
        
        var cellView = cellObject.GetComponent<CellView>();
        cellView.Init(cell);
    }
}