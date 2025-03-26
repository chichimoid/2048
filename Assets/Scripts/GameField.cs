using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
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
    public List<Cell> Cells { get; } = new(); // Personally, wouldn't make that a list, but that was the requirement.
    public static GameField Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _width = GameFieldDataTransmitter.Instance.Width;
        _height = GameFieldDataTransmitter.Instance.Height;
        
        _xOffset = (float)-(_width - 1) / 2;
        _yOffset = (float)-(_height - 1) / 2;
    }
    
    private void Start()
    {
        _gameFieldTransform = GetComponent<RectTransform>();
        
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
        if (Cells.Count == _width * _height)
        {
            Debug.Log("Game field is full");
            return;
        }
        
        int valueDistr = Random.Range(1, 11); // Hoping that Unity.Random gives a uniform distribution.
        int value = valueDistr == 1 ? 2 : 1;
        
        var pos = GetEmptyPosition();
        
        var cell = new Cell(pos, value);

        Cells.Add(cell);
        
        var cellObject = Instantiate(valueCellPrefab.transform, transform);
        
        var cellView = cellObject.GetComponent<CellView>();
        cellView.Init(cell);
    }
}