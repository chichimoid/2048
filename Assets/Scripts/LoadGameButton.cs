using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour
{
    private Button _button;
    private TextDisplayer _textDisplayer;

    private void Awake()
    {
        _textDisplayer = GetComponent<TextDisplayer>();
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadGame);
    }

    private void LoadGame()
    {
        if (!SaveManager.Instance.TryLoadData(out var state))
        {
            _textDisplayer.TempDisplayText("Nothing to load!");
            return;
        }
        
        GameStateStaticContainer.GameState = state;
        
        SceneManager.LoadScene("Game");
    }
}