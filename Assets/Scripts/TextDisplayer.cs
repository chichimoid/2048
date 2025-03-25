using System.Collections;
using TMPro;
using UnityEngine;

public class TextDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    private string _prevText;
    private Coroutine _hideTextCoroutine;
        
    public void TempDisplayText(string text, float duration = 1f)
    {
        _prevText = textField.text;
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
        textField.text = _prevText;
        _hideTextCoroutine = null;
    }
}