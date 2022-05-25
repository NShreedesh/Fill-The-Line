using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnner : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private LevelObject levelObject;

    [Header("Spawn Level Info")]
    private Transform _tileContainer;
    [HideInInspector] public List<Tile> tileList = new();
    [HideInInspector] public Tile firstTile;

    [Header("Tile Storage Info")]
    public List<Tile> filledTileList = new();

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
    private Color _tileColor;

    private void Start()
    {
        SpawnLevel();
        GameManager.Instance.ChangeGameState(GameManager.GameState.Play);
        GameManager.Instance.AssignTotalNumberOfLevel(levelObject.levelInfos.Count);
    }

    private void SpawnLevel()
    {
        _tileColor = tileColorOptions[Random.Range(0, tileColorOptions.Length)];
        LevelInfo levleInfo = levelObject.levelInfos[GameManager.Instance.LevelNumber - 1];

        _tileContainer = Instantiate(levleInfo.levelPrefab, transform.position, Quaternion.identity).transform;

        for (int i = 0; i < _tileContainer.childCount; i++)
        {
            _tileContainer.GetChild(i).TryGetComponent<Tile>(out Tile tile);
            tile.fillColor = _tileColor;
            tileList.Add(tile);
        }

        firstTile = levleInfo.selectFirstTile < _tileContainer.childCount ? tileList[levleInfo.selectFirstTile] : tileList[0];

        firstTile.Fill_UnFill_Tile(true);

        filledTileList.Add(firstTile);
    }
}
