using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevelButtonSpawn : MonoBehaviour
{
    [Header("Level Button Info")]
    [SerializeField] private Button levelButtonPrefab;
    [SerializeField] private int numberOfButtonsToSpawnInOneScreen = 12;

    private void Start()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Menu);
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < numberOfButtonsToSpawnInOneScreen; i++)
        {
            Button button = Instantiate(levelButtonPrefab, transform);
            LevelButton levelButton = button.GetComponent<LevelButton>();

            int levelNumber = i + 1;
            levelButton.levelNumberText.text = levelNumber.ToString();

            if (GameManager.Instance.TotalUnlockedLevels < levelNumber)
            {
                button.interactable = false;
                levelButton.lockImage.gameObject.SetActive(true);
                levelButton.levelNumberText.gameObject.SetActive(false);
                continue;
            }

            levelButton.lockImage.gameObject.SetActive(false);
            levelButton.levelNumberText.gameObject.SetActive(true);

            button.onClick.AddListener(() => ButtonFunction(levelNumber));
        }
    }

    private void ButtonFunction(int levelNumber)
    {
        GameManager.Instance.AssignLevelNumber(levelNumber);
        SceneManager.LoadScene(1);
    }
}
