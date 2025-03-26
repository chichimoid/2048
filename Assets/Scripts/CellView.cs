using System;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CellView : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    
    private int _fieldWidth = 4;
    private int _fieldHeight = 4;
    private float _xOffset;
    private float _yOffset;
    private float _scaleMult = 1;
    
    private float _cellDistance = 64;
    private Cell _cell;
    private RectTransform _rectTransform;

    public void Init(Cell cell)
    {
        _rectTransform = GetComponent<RectTransform>();
        
        _fieldWidth = GameField.Instance.Width;
        _fieldHeight = GameField.Instance.Height;
        _cellDistance = GameField.Instance.CellDistance;
        _scaleMult = GameField.Instance.CellScaleMult;
        _xOffset = GameField.Instance.XOffset;
        _yOffset = GameField.Instance.YOffset;
        
        if (_fieldWidth != 4 || _fieldHeight != 4)
        {
            _rectTransform.sizeDelta *= new Vector2(_scaleMult, _scaleMult);
        }
        
        _cell = cell;
        _cell.OnPositionChanged += UpdatePosition;
        _cell.OnValueChanged += UpdateValue;
        
        UpdatePosition();
        UpdateValue();
    }

    private void UpdatePosition()
    {
        var position = _cell.Position;
        _rectTransform.anchoredPosition = new Vector2(
            (_xOffset + position.x) * _cellDistance, 
            (_yOffset + position.y) * _cellDistance);
    }

    private void UpdateValue()
    {
        textField.text = Math.Pow(2, _cell.Value).ToString(CultureInfo.InvariantCulture);
    }
}