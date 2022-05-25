using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Button Info")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button soundButton;

    [Header("Level Text Info")]
    [SerializeField] private TMP_Text levelText;

    [Header("Sound Button Info")]
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("Audio Info")]
    [SerializeField] private AudioClip clickAudioClip;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        soundButton.onClick.AddListener( OnSoundButtonClicked);
       
        ChangeLevelText();
        ChangeSoundSprite();
    }

    private void OnHomeButtonClicked()
    {
        AudioManager.Instance.PlayEffectAudio(clickAudioClip);
        SceneManager.LoadScene(0);
    }

    private void OnSoundButtonClicked()
    {
        AudioManager.Instance.PlayEffectAudio(clickAudioClip);
        AudioManager.Instance.SetSound();
        ChangeSoundSprite();
    }

    private void ChangeSoundSprite()
    {
        if (AudioManager.IsSoundOn)
        {
            soundButton.image.sprite = soundOnSprite;
        }
        else
        {
            soundButton.image.sprite = soundOffSprite;
        }
    }

    private void ChangeLevelText()
    {
        levelText.text = GameManager.Instance.LevelNumber.ToString();
    }
}
