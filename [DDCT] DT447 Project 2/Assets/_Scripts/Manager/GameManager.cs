using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { PLAY, PAUSED }

    public static GameManager Instance;
    [field: SerializeField] public GameState gameState { get; private set; } = GameState.PLAY;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused())
            Cursor.lockState = CursorLockMode.None;
    }

    public bool IsPaused()
    {
        return gameState == GameState.PAUSED ? true : false;
    }

    public void Resume()
    {
        gameState = GameState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        gameState = GameState.PAUSED;
        Cursor.lockState = CursorLockMode.None;
    }
}
