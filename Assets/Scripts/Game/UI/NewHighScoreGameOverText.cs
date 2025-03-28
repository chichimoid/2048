using UnityEngine;

namespace Game.UI
{
    public class NewHighScoreGameOverText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject newHighScoreTextField;
        private void Start()
        {
            GameManager.Instance.OnNewHighScoreGameOver += Activate;
        
            newHighScoreTextField.SetActive(false);
        }

        private void Activate()
        {
            newHighScoreTextField.SetActive(true);
        }
    }
}