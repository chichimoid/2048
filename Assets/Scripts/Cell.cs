using UnityEngine;

public class Cell
{
    public Cell(Vector2Int position, int value)
    {
        _position = position;
        _value = value;
    }
    
    private Vector2Int _position ;
    private int _value;
    
    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPositionChanged?.Invoke();
        } 
    }
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke();
        } 
    }
    
    public delegate void OnPositionChangedDelegate();
    public event OnPositionChangedDelegate OnPositionChanged;
    
    public delegate void OnValueChangedDelegate();
    public event OnValueChangedDelegate OnValueChanged;
}
