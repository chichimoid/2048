using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenuButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {   
        _button = GetComponent<Button>();
        _button.onClick.AddListener(BackToMenu);
    }

    private void BackToMenu()
    {
        GameStateStaticContainer.GameState.width = 0;
        GameStateStaticContainer.GameState.height = 0;
        GameManager.Instance.StopGame();

        SceneManager.LoadScene("Menu");
    }
}