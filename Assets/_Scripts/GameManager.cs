using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("GameState Info")]
    public GameState gameState;

    [Header("Game Level Info")]
    [Min(1)]
    public int levelNumber;

    public event Action OnMenuState;
    public event Action OnPlayState;
    public event Action OnWinState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        OnWinState += OnGameWin;
    }

    private void OnDisable()
    {
        OnWinState -= OnGameWin;
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;

        switch (gameState)
        {
            case GameState.Menu:
                OnMenuState?.Invoke();
                break;
            case GameState.Play:
                OnPlayState?.Invoke();
                break;
            case GameState.Win:
                OnWinState?.Invoke();
                break;
            default:
                break;
        }
    }

    private async void OnGameWin()
    {
        await Task.Delay(2000);
        SceneManager.LoadSceneAsync(2);
    }

    public enum GameState
    {
        Menu,
        Play,
        Win
    }
}
