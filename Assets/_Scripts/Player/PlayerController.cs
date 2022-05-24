using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Other Scripts Info")]
    InputController inputController;
    [SerializeField] private LevelSpawnner spawnner;

    [Header("Input Info")]
    private float clickInputValue;

    private void Awake()
    {
        inputController = new InputController();
    }

    private void OnEnable()
    {
        inputController.Player.Click.started += OnClicked;
        inputController.Player.Click.canceled += OnReleased;

        inputController.Enable();
    }

    private void OnClicked(CallbackContext ctx)
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Play) return;
        clickInputValue = ctx.ReadValue<float>();
    }

    private void OnReleased(CallbackContext ctx)
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Play) return;

        clickInputValue = ctx.ReadValue<float>();

        spawnner.CheckAllTiles();
    }

    private void Update()
    {
        if (clickInputValue != 1) return;

        Vector2 worldMosuePosition = spawnner.ConvertToWorldPosition(inputController.Player.MousePosition.ReadValue<Vector2>());
        spawnner.FillTheTile(worldMosuePosition);
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
}
