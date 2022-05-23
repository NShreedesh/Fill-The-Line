using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIButtons : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        homeButton.onClick.AddListener(() =>
        {
            OnHomeButtonClicked();
        });
        playButton.onClick.AddListener(() =>
        {
            OnPlayButtonClicked();
        });
        restartButton.onClick.AddListener(() =>
        {
            OnRestartButtonClicked();
        });
    }

    private void OnHomeButtonClicked()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.SaveLevel();
        SceneManager.LoadSceneAsync(1);
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }
}
