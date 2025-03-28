using UnityEngine;
using Utility;

namespace Menu
{
    public class MenuInputValidator : MonoBehaviour
    {
        [Header("Configure")] 
        [SerializeField] private int maxWidth = 7;
        [SerializeField] private int maxHeight = 7;
    
        [Header("References")]
        [SerializeField] private TextDisplayer textDisplayer;
    
        public static MenuInputValidator Instance;

        private void Awake()
        {
            Instance = this;
        }

        public bool Validate(string widthInput, string heightInput, out int width, out int height)
        {
            if (!int.TryParse(widthInput, out width) || width < 1 || width > maxWidth)
            {
                textDisplayer.TempDisplayText($"Enter a valid width from 1 to {maxWidth}");
                height = default;
                return false;
            }
        
            if (!int.TryParse(heightInput, out height) || height < 1 || height > maxHeight)
            {
                textDisplayer.TempDisplayText($"Enter a valid height from 1 to 7{maxHeight}");
                return false;
            }

            return true;
        }
    }
}