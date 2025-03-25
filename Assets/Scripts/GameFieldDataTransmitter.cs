using UnityEngine;

public class GameFieldDataTransmitter : MonoBehaviour
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public static GameFieldDataTransmitter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}