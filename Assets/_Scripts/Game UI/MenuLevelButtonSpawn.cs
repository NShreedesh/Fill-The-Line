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
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            int levelNumber = i + 1;
            buttonText.text = levelNumber.ToString();

            if (GameManager.Instance.totalUnlockedLevels < levelNumber)
            {
                button.interactable = false;
                continue;
            }

            button.onClick.AddListener(() => ButtonFunction(levelNumber));
        }
    }

    private void ButtonFunction(int levelNumber)
    {
        GameManager.Instance.levelNumber = levelNumber;
        SceneManager.LoadScene(1);
    }
}
