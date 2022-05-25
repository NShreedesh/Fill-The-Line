using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnner : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private List<GameObject> levelList = new();

    [Header("Spawn Level Info")]
    private Transform tileContainer;
    [HideInInspector] public List<Tile> tileList = new();
    [HideInInspector] public Tile firstTile;
    [HideInInspector] public Vector2 prevPostion;

    [Header("Tile Storage Info")]
    public Tile currentTile;

    [Header("Camera Info")]
    public Camera cam;

    [Header("Tile Color Info")]
    [SerializeField] private Color[] tileColorOptions =
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
        GameManager.Instance.AssignTotalNumberOfLevel(levelList.Count);
    }

    private void SpawnLevel()
    {
        tileColor = tileColorOptions[Random.Range(0, tileColorOptions.Length)];

        tileContainer = Instantiate(levelList[GameManager.Instance.LevelNumber - 1], transform.position, Quaternion.identity).transform;

        for (int i = 0; i < tileContainer.childCount; i++)
        {
            tileContainer.GetChild(i).TryGetComponent<Tile>(out Tile tile);
            tile.fillColor = tileColor;
            tileList.Add(tile);
        }

        firstTile = tileList[0];

        prevPostion = firstTile.transform.position;
        firstTile.Fill_UnFill_Tile(true);
    }
}
