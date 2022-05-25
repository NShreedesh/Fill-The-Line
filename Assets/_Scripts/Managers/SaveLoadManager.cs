using UnityEngine;

public static class SaveLoadManager
{
    public static void Save(int levelNumber)
    {
        PlayerPrefs.SetInt(SaveLoadManagerTags.LevelNumberTag, levelNumber);
    }

    public static int Load(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return 1;

        return PlayerPrefs.GetInt(key);
    }
}
