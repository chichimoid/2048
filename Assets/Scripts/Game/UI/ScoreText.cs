using TMPro;
using UnityEngine;

namespace Game.UI
{
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
}