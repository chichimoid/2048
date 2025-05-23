using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    
    private Button _button;
    private TextDisplayer _textDisplayer;
        
    private void Awake()
    {
        _textDisplayer = GetComponent<TextDisplayer>();
    }

    private void Start()
    {   
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartGame(widthInputField.text, heightInputField.text));
    }

    private void StartGame(string fieldWidthInput, string fieldHeightInput)
    {
        if (!int.TryParse(fieldWidthInput, out int width) || width < 1 || width > 7)
        {
            _textDisplayer.TempDisplayText("Enter a valid width from 1 to 7");
            return;
        }
        
        if (!int.TryParse(fieldHeightInput, out int height) || height < 1 || height > 7)
        {
            _textDisplayer.TempDisplayText("Enter a valid height from 1 to 7");
            return;
        }
        
        GameFieldDataTransmitter.Instance.Width = width;
        GameFieldDataTransmitter.Instance.Height = height;

        SceneManager.LoadScene("Game");
    }
}
