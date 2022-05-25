using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Storage", menuName = "Level Data/Level Strorage", order = 1)]
public class LevelObject : ScriptableObject
{
    public List<LevelInfo> levelInfos = new();
}


[System.Serializable]
public class LevelInfo
{
    [SerializeField] private string levelNumber;
    public GameObject levelPrefab;
    public int selectFirstTile;
}