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
        GameFieldDataTransmitter.Instance.Width = 0;
        GameFieldDataTransmitter.Instance.Height = 0;

        SceneManager.LoadScene("Menu");
    }
}