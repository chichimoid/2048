using GlobalData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class NewGameButton : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private TMP_InputField widthInputField;
        [SerializeField] private TMP_InputField heightInputField;
    
        private Button _button;

        private void Start()
        {   
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => NewGame(widthInputField.text, heightInputField.text));
        }

        private void NewGame(string fieldWidthInput, string fieldHeightInput)
        {
            if (!MenuInputValidator.Instance.Validate(fieldWidthInput, fieldHeightInput, out int width, out int height)) return;
        
            GameStateStaticContainer.GameState.width = width;
            GameStateStaticContainer.GameState.height = height;
            GameStateStaticContainer.GameState.cells = null;
        
            GameStateStaticContainer.GameState.highScore = SaveManager.Instance.TryLoadGameState(width, height, out var state) ? state.highScore : 0;

            SceneManager.LoadScene("Game");
        }
    }
}
