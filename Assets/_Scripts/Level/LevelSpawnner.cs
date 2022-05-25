using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnner : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private LevelObject levelObject;

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
        GameManager.Instance.AssignTotalNumberOfLevel(levelObject.levelInfos.Count);
    }

    private void SpawnLevel()
    {
        tileColor = tileColorOptions[Random.Range(0, tileColorOptions.Length)];
        LevelInfo levleInfo = levelObject.levelInfos[GameManager.Instance.LevelNumber - 1];

        tileContainer = Instantiate(levleInfo.levelPrefab, transform.position, Quaternion.identity).transform;

        for (int i = 0; i < tileContainer.childCount; i++)
        {
            tileContainer.GetChild(i).TryGetComponent<Tile>(out Tile tile);
            tile.fillColor = tileColor;
            tileList.Add(tile);
        }

        firstTile = levleInfo.selectFirstTile < tileContainer.childCount ? tileList[levleInfo.selectFirstTile] : tileList[0];

        prevPostion = firstTile.transform.position;
        firstTile.Fill_UnFill_Tile(true);
    }
}
