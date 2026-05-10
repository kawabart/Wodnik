using UnityEngine;

public static class SaveManager
{
    public static void SaveLevelResult(int levelIndex, int result)
    {
        PlayerPrefs.SetInt(
            $"Level_{levelIndex}_Completed",
            1);

        PlayerPrefs.SetInt(
            $"Level_{levelIndex}_Infamy",
            result);

        PlayerPrefs.Save();
    }

    public static bool HasLevelResult(int levelIndex)
    {
        return PlayerPrefs.HasKey(
            $"Level_{levelIndex}_Infamy");
    }

    public static int GetLevelInfamy(int levelIndex)
    {
        return PlayerPrefs.GetInt(
            $"Level_{levelIndex}_Infamy",
            0);
    }

    public static bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt(
            $"Level_{levelIndex}_Completed",
            0) == 1;
    }
}
