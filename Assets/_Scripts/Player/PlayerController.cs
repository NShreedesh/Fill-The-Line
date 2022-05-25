using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Other Scripts Info")]
    private InputController inputController;
    [SerializeField] private LevelSpawnner spawnner;

    [Header("Input Info")]
    private float clickInputValue;

    [Header("Click Info")]
    [SerializeField] private bool isFirstClick = true;

    [Header("Tile Info")]
    public LayerMask tileLayer;

    [Header("Effect Info")]
    [SerializeField] private ParticleSystem winEffect;

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

        CheckAllTiles();
    }

    private void Update()
    {
        if (clickInputValue != 1) return;

        Vector2 worldMosuePosition = ConvertToWorldPosition(inputController.Player.MousePosition.ReadValue<Vector2>());
        FillTheTile(worldMosuePosition);
    }

    public void FillTheTile(Vector2 worldMousePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero, 100, tileLayer);

        if (hit.collider == null) return;
        Tile tile = hit.transform.GetComponent<Tile>();

        if (isFirstClick)
        {
            if (tile == spawnner.firstTile)
            {
                isFirstClick = false;
            }
            else
            {
                return;
            }
        }

        if (tile.isFilled) return;
        if (!CheckIftheTileCanBeFilled(tile.transform.position)) return;
        spawnner.currentTile = tile;
        tile.Fill_UnFill_Tile(true);
    }

    private void UnFillAllTiles()
    {
        foreach (Tile tile in spawnner.tileList)
        {
            if (tile != spawnner.firstTile)
            {
                tile.Fill_UnFill_Tile(false);
            }
        }

        spawnner.prevPostion = spawnner.firstTile.transform.position;
        isFirstClick = true;
    }

    public void CheckAllTiles()
    {
        foreach (Tile tile in spawnner.tileList)
        {
            if (!tile.isFilled)
            {
                UnFillAllTiles();
                return;
            }
        }
        Instantiate(winEffect, spawnner.currentTile.transform.position, Quaternion.identity);
        GameManager.Instance.ChangeGameState(GameManager.GameState.Win);
    }

    public bool CheckIftheTileCanBeFilled(Vector2 position)
    {
        if (Mathf.Abs(spawnner.prevPostion.x - position.x) == 1 &&
           Mathf.Abs(spawnner.prevPostion.y - position.y) == 0)
        {
            spawnner.prevPostion = position;
            return true;
        }

        else if (Mathf.Abs(spawnner.prevPostion.y - position.y) == 1 &&
           Mathf.Abs(spawnner.prevPostion.x - position.x) == 0)
        {
            spawnner.prevPostion = position;
            return true;
        }

        return false;
    }

    public Vector2 ConvertToWorldPosition(Vector2 position)
    {
        Vector2 worldPosition = spawnner.cam.ScreenToWorldPoint(position);
        return worldPosition;
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
}
