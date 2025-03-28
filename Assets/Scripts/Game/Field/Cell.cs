using System;
using UnityEngine;

namespace Game.Field
{
    [Serializable]
    public class Cell
    {
        public Cell(Vector2Int position, int value)
        {
            this.position = position;
            this.value = value;
        }
    
        [SerializeField] Vector2Int position ;
        [SerializeField] private int value;

        public Vector2Int Position
        {
            get => position;
            set
            {
                position = value;
                OnPositionChanged?.Invoke();
            } 
        }
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke();
            } 
        }
    
        public delegate void OnPositionChangedDelegate();
        public event OnPositionChangedDelegate OnPositionChanged;
    
        public delegate void OnValueChangedDelegate();
        public event OnValueChangedDelegate OnValueChanged;
    
        public delegate void OnCellDestroyedDelegate();
        public event OnCellDestroyedDelegate OnCellDestroyed;

        public void Destroy()
        {
            OnCellDestroyed?.Invoke();
        }
    }
}
