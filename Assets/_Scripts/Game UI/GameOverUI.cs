using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Buttons Info")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;

    [Header("Audio Info")]
    [SerializeField] private AudioClip clickAudioClip;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        playButton.onClick.AddListener(OnPlayButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnHomeButtonClicked()
    {
        AudioManager.Instance.PlayEffectAudio(clickAudioClip);
        SceneManager.LoadSceneAsync(0);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.IncrementLevel();
        AudioManager.Instance.PlayEffectAudio(clickAudioClip);
        SceneManager.LoadScene(1);
    }

    private void OnRestartButtonClicked()
    {
        AudioManager.Instance.PlayEffectAudio(clickAudioClip);
        SceneManager.LoadScene(1);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }
}
