using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool hasMap = false;
    public enum GameState { PLAY, PAUSED }

    public static GameManager Instance;
    [field: SerializeField] public GameState gameState { get; private set; } = GameState.PLAY;

    [SerializeField] private GameObject mapPanel;

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _respawnLocation;

    private void Awake()
    {
        Instance = this;
    }

    public void Respawn()
    {
        _player.SetActive(false);
        _player.transform.position = _respawnLocation.position;
        _player.SetActive(true);
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

        if (Input.GetKeyDown(KeyCode.M) && hasMap)
        {
            if (gameState == GameState.PLAY)
                Pause();
            else
                Resume();

            mapPanel.SetActive(!mapPanel.activeSelf);
        }
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

    public void AquireMap()
    {
        hasMap = true;
    }
}
