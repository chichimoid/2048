using UnityEngine;

namespace Game.UI
{
    public class GameOverPopUp : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject gameOverPanel;
    
        private void Start()
        {
            GameManager.Instance.OnGameEnded += Activate;
        
            gameOverPanel.SetActive(false);
        }

        private void Activate()
        {
            gameOverPanel.SetActive(true);
        }
    }
}