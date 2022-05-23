using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnner : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private List<GameObject> levelList = new List<GameObject>();

    [Header("Spawn Level Info")]
    public LayerMask tileLayer;
    private Transform tileContainer;
    [HideInInspector] public List<Tile> tileList = new();
    [HideInInspector] public Tile firstTile;
    [HideInInspector] public Vector2 prevPostion;

    [Header("Camera Info")]
    public Camera cam;

    [Header("Effect Info")]
    [SerializeField] private ParticleSystem winEffect;

    [Header("Click Info")]
    [SerializeField] private bool isFirstClick = true;

    [Header("Tile Color Info")]
    private Color[] tileColorOptions =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta
    };
    private Color tileColor;

    private void Start()
    {
        SpawnLevel();
        GameManager.Instance.ChangeGameState(GameManager.GameState.Play);
    }

    private void SpawnLevel()
    {
        tileColor = tileColorOptions[Random.Range(0, tileColorOptions.Length)];

        tileContainer = Instantiate(levelList[GameManager.Instance.levelNumber - 1], transform.position, Quaternion.identity).transform;

        for (int i = 0; i < tileContainer.childCount; i++)
        {
            tileContainer.GetChild(i).TryGetComponent<Tile>(out Tile tile);
            tile.fillColor = tileColor;
            tileList.Add(tile);
        }

        firstTile = tileList[0];

        prevPostion = firstTile.transform.position;
        firstTile.FillTile(true);
    }

    public void FillTheTile(Vector2 worldMousePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero, 100, tileLayer);

        if (hit.collider == null) return;
        Tile tile = hit.transform.GetComponent<Tile>();

        if (isFirstClick)
        {
            if (tile == firstTile)
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
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        tile.FillTile(true);
    }

    private void UnFillAllTiles()
    {
        foreach (Tile tile in tileList)
        {
            if (tile != firstTile)
            {
                tile.FillTile(false);
            }
        }

        prevPostion = firstTile.transform.position;
        isFirstClick = true;
    }

    public void CheckAllTiles(Vector2 worldMousePosition)
    {
        foreach (Tile tile in tileList)
        {
            if (!tile.isFilled)
            {
                UnFillAllTiles();
                return;
            }
        }
        Instantiate(winEffect, worldMousePosition, Quaternion.identity);
        GameManager.Instance.ChangeGameState(GameManager.GameState.Win);
    }

    private bool CheckIftheTileCanBeFilled(Vector2 position)
    {
        if (Mathf.Abs(prevPostion.x - position.x) == 1 &&
           Mathf.Abs(prevPostion.y - position.y) == 0)
        {
            prevPostion = position;
            return true;
        }

        else if (Mathf.Abs(prevPostion.y - position.y) == 1 &&
           Mathf.Abs(prevPostion.x - position.x) == 0)
        {
            prevPostion = position;
            return true;
        }

        return false;
    }

    public Vector2 ConvertToWorldPosition(Vector2 position)
    {
        Vector2 worldPosition = cam.ScreenToWorldPoint(position);
        return worldPosition;
    }
}
