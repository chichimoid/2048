using System;
using System.Collections;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    private static readonly int MoveCellStartPosX = Animator.StringToHash("MoveCellStartPosX");
    private static readonly int MoveCellStartPosY = Animator.StringToHash("MoveCellStartPosY");
    private static readonly int MoveCellEndPosX = Animator.StringToHash("MoveCellEndPosX");
    private static readonly int MoveCellEndPosY = Animator.StringToHash("MoveCellEndPosY");

    [Header("Configure")] 
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private int endValue = 11;
    
    [Header("References")]
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Image image;
    
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
        _cell.OnCellDestroyed+= DestroyView;
        
        UpdatePosition();
        UpdateValue();
    }

    private void UpdatePosition()
    {
        var targetPosition = _cell.Position;
        _rectTransform.anchoredPosition = new Vector2(
            (_xOffset + targetPosition.x) * _cellDistance,
            (_yOffset + targetPosition.y) * _cellDistance);
    }

    private void UpdateValue()
    {
        image.color = Color.Lerp(startColor, endColor, (float)_cell.Value / endValue);
        Debug.Log((float)_cell.Value / endValue);
        textField.text = Math.Pow(2, _cell.Value).ToString(CultureInfo.InvariantCulture);
    }

    private void DestroyView()
    {
        Destroy(this.GameObject());
    }
}