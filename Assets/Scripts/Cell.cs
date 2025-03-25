public class Cell
{
    public Cell(Coordinates position, int value)
    {
        _position = position;
        _value = value;
    }
    
    private Coordinates _position ;
    private int _value;
    
    public Coordinates Position
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
