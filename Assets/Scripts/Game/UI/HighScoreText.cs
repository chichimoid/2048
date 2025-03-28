using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HighScoreText : MonoBehaviour
    {
        private TMP_Text _textField;

        private void Start()
        {
            _textField = GetComponent<TMP_Text>();
        
            GameManager.Instance.OnHighScoreChanged += UpdateScore;
        }

        private void UpdateScore(int score)
        {
            _textField.text = score.ToString();
        }
    }
}