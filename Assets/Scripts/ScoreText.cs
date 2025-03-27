using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TMP_Text _textField;

    private void Start()
    {
        _textField = GetComponent<TMP_Text>();
        
        GameManager.Instance.OnScoreChanged += UpdateScore;
    }

    private void UpdateScore(int score)
    {
        _textField.text = score.ToString();
    }
}