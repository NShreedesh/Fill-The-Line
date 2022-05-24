using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
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
    private int levelNumber = 1;
    public int LevelNumber { get { return levelNumber; }}
    [Min(1)]
    private int totalNumberofLevels = 1;
    public int TotalNumberofLevels { get { return totalNumberofLevels; }}
    [Min(1)]
    private int totalUnlockedLevels = 1;
    public int TotalUnlockedLevels { get { return totalUnlockedLevels; } }

    [Header("Level Timing Info")]
    [SerializeField] private float timeForLoadingWinLevel;

    public event Action OnMenuState;
    public event Action OnPlayState;
    public event Action OnWinState;

    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        SetFrameRateAccordingToPlatform();

        if (instance == null)
        {
            instance = this;
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
        cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Token.ThrowIfCancellationRequested();
        try
        {
            await Task.Delay((int)(timeForLoadingWinLevel * 1000), cancellationTokenSource.Token);
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
        if (PlayerPrefs.HasKey(SaveLoadManagerTags.LevelNumberTag))
        {
            totalUnlockedLevels = PlayerPrefs.GetInt(SaveLoadManagerTags.LevelNumberTag);
        }
        else
        {
            totalUnlockedLevels = 1;
        }
    }

    public void IncrementLevel()
    {
        if (levelNumber < totalNumberofLevels)
        {
            levelNumber++;
        }
    }

    public void SaveLevel()
    {
        if (levelNumber >= totalNumberofLevels) return;
        int levelToSave = levelNumber + 1;

        if (levelToSave <= totalUnlockedLevels) return;
        SaveLoadManager.Save(levelToSave);
    }

    public void AssignLevelNumber(int level)
    {
        levelNumber = level;
    }

    public void AssignTotalNumberOfLevel(int totalLevels)
    {
        totalNumberofLevels = totalLevels;
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
