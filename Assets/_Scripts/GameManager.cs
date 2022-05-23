using System;
using System.Threading;
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
    [SerializeField] private int totalNumberofLevels;

    [Header("Level Timing Info")]
    [SerializeField] private float timeForLoadingWinLevel;

    public event Action OnMenuState;
    public event Action OnPlayState;
    public event Action OnWinState;

    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        levelNumber = SaveLoadManager.Load("LevelNumber");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        OnWinState += OnGameWin;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
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
        cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Token.ThrowIfCancellationRequested();
        try
        {
            await Task.Delay((int)(timeForLoadingWinLevel * 1000), cancellationTokenSource.Token);
            SceneManager.LoadScene(2);
        }
        catch
        {
            print("Task is Cancelled because Application has Shutdown...");
        }
    }

    public void SaveLevel()
    {
        if (levelNumber < totalNumberofLevels)
        {
            levelNumber++;
        }

        SaveLoadManager.Save(levelNumber);
    }

    private void OnApplicationQuit()
    {
        cancellationTokenSource?.Cancel();
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        cancellationTokenSource?.Cancel();
    }

    public enum GameState
    {
        Menu,
        Play,
        Win
    }
}
