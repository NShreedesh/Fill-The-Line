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
    [SerializeField] private bool isSoundOn;
    public bool IsSoundOn
    {
        get { return isSoundOn; }
        private set
        {
            isSoundOn = value;
            AudioListener.pause = !isSoundOn;
            ChangeSoundSprite();
        }
    }
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        soundButton.onClick.AddListener( OnSoundButtonClicked);

        IsSoundOn = true;
        ChangeLevelText();
    }

    private void OnHomeButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    private void OnSoundButtonClicked()
    {
        IsSoundOn = !IsSoundOn;
    }

    private void ChangeSoundSprite()
    {
        if (isSoundOn)
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
