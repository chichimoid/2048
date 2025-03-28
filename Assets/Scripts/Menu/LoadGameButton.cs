using GlobalData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Menu
{
    public class LoadGameButton : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private TMP_InputField widthInputField;
        [SerializeField] private TMP_InputField heightInputField;
    
        [Header("References")]
        [SerializeField] private TextDisplayer textDisplayer;
    
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => LoadGame(widthInputField.text, heightInputField.text));
        }

        private void LoadGame(string fieldWidthInput, string fieldHeightInput)
        {
            if (!MenuInputValidator.Instance.Validate(fieldWidthInput, fieldHeightInput, out int width, out int height)) return;
        
            if (!SaveManager.Instance.TryLoadGameState(width, height, out var state) || state.cells.Count == 0)
            {
                textDisplayer.TempDisplayText("Nothing to load!");
                return;
            }
        
            GameStateStaticContainer.GameState = state;
        
            SceneManager.LoadScene("Game");
        }
    }
}