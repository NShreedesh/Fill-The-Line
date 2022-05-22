using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ClickController : MonoBehaviour
{
    InputController inputController;

    [SerializeField] private float clickInputValue;
    [SerializeField] private Camera cam;

    [SerializeField] private List<Tile> tileList = new();
    [SerializeField] private Vector2 prevPostion;
    [SerializeField] private Tile firstTile;

    private void Awake()
    {
        inputController = new InputController();
    }

    private void Start()
    {
        prevPostion = firstTile.transform.position;
        firstTile.HandleTile(true);
    }

    private void OnEnable()
    {
        inputController.Player.Click.started += OnClicked;
        inputController.Player.Click.canceled += OnReleased;

        inputController.Enable();
    }

    private void OnClicked(CallbackContext ctx)
    {
        clickInputValue = ctx.ReadValue<float>();
    }

    private void OnReleased(CallbackContext ctx)
    {
        clickInputValue = ctx.ReadValue<float>();
        CheckAllTiles();
    }

    private void Update()
    {
        if (clickInputValue == 1)
        {
            FillTheTile();
        }
    }

    private void FillTheTile()
    {
        Vector2 worldMousePosition = cam.ScreenToWorldPoint(inputController.Player.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);

        if (hit.collider == null) return;
        Tile tile = hit.transform.GetComponent<Tile>();

        if (tile.isFilled) return;
        if (!CheckIftheTileCanBeFilled(tile.transform.position)) return;
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        tile.HandleTile(true);
    }

    private void UnFillAllTiles()
    {
        foreach(Tile tile in tileList)
        {
            if(tile != firstTile)
            {
                tile.HandleTile(false);
            }
        }

        prevPostion = firstTile.transform.position;
    }

    private void CheckAllTiles()
    {
        foreach(Tile tile in tileList)
        {
            if (!tile.isFilled)
            {
                UnFillAllTiles();
                return;
            }
        }

        print("Won");
    }

    private bool CheckIftheTileCanBeFilled(Vector2 position)
    {
        if(Mathf.Abs(prevPostion.x - position.x) == 1 &&
           Mathf.Abs(prevPostion.y - position.y) == 0)
        {
            prevPostion = position;
            return true;
        }

        else if(Mathf.Abs(prevPostion.y - position.y) == 1 &&
           Mathf.Abs(prevPostion.x - position.x) == 0)
        {
            prevPostion = position;
            return true;
        }

        return false;
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
}
