using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("GameState Info")]
    public GameState gameState;

    [Header("Game Level Info")]
    [Min(1)]
    private int _levelNumber = 1;
    public int LevelNumber { get { return _levelNumber; }}
    [Min(1)]
    private int _totalNumberofLevels = 1;
    public int TotalNumberofLevels { get { return _totalNumberofLevels; }}
    [Min(1)]
    private int _totalUnlockedLevels = 1;
    public int TotalUnlockedLevels { get { return _totalUnlockedLevels; } }

    [Header("Level Timing Info")]
    [SerializeField] private float timeForLoadingWinLevel;

    public event Action OnMenuState;
    public event Action OnPlayState;
    public event Action OnWinState;

    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        SetFrameRateAccordingToPlatform();

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void SetFrameRateAccordingToPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                break;
            default:
                Application.targetFrameRate = -1;
                break;
        }
    }

    private void Start()
    {
        UnlockedLevelChange();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        OnPlayState += UnlockedLevelChange;
        OnMenuState += UnlockedLevelChange;

        OnWinState += OnGameWin;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        OnPlayState -= UnlockedLevelChange;
        OnMenuState -= UnlockedLevelChange;

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
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        try
        {
            await Task.Delay((int)(timeForLoadingWinLevel * 1000), _cancellationTokenSource.Token);
            SceneManager.LoadScene(2);
        }
        catch
        {
#if UNITY_EDITOR
            print("Task is Cancelled because Application has Shutdown...");
#endif
        }
        finally
        {
            SaveLevel();
        }
    }

    private void UnlockedLevelChange()
    {
        _totalUnlockedLevels = SaveLoadManager.Load(SaveLoadManagerTags.LevelNumberTag);
    }

    public void IncrementLevel()
    {
        if (_levelNumber < _totalNumberofLevels)
        {
            _levelNumber++;
        }
    }

    public void SaveLevel()
    {
        if (_levelNumber >= _totalNumberofLevels) return;
        int levelToSave = _levelNumber + 1;

        if (levelToSave <= _totalUnlockedLevels) return;
        SaveLoadManager.Save(levelToSave);
    }

    public void AssignLevelNumber(int level)
    {
        _levelNumber = level;
    }

    public void AssignTotalNumberOfLevel(int totalLevels)
    {
        _totalNumberofLevels = totalLevels;
    }

    private void OnApplicationQuit()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        _cancellationTokenSource?.Cancel();
    }

    public enum GameState
    {
        Menu,
        Play,
        Win
    }
}
