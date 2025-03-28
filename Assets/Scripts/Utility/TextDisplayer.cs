using System.Collections;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class TextDisplayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text textField;
    
        private Coroutine _hideTextCoroutine;
        
        public void TempDisplayText(string text, float duration = 1f)
        {
            if (_hideTextCoroutine != null)
            {
                StopCoroutine(_hideTextCoroutine);
            }
        
            textField.text = text;
            _hideTextCoroutine = StartCoroutine(HideTextAfterDelay(duration));
        }
    
        private IEnumerator HideTextAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            textField.text = "";
            _hideTextCoroutine = null;
        }
    }
}