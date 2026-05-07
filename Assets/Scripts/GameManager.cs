using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        transform.parent = null;
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("GameManager initialized");
        CurrentLevel = GetCurrentLevelIndex();
        Debug.Log("Current level is " + CurrentLevel);
    }

    #region levels
    public int CurrentLevel = 0;
    public string[] LevelNames;
    private int GetCurrentLevelIndex()
    {
        for (int i = 0; i < LevelNames.Length; i++)
        {
            if (LevelNames[i] == SceneManager.GetActiveScene().name) return i;
        }
        return 0;
    }
    public void CompleteLevel()
    {
        CurrentLevel++;
        if (CurrentLevel >= LevelNames.Length)
        {
            Debug.Log("CONGRATS! You won! Game Over (in a good way). Achievement unlocked: Happily ever after...");
        }
        else
        {
            SceneManager.LoadScene(LevelNames[CurrentLevel]);
        }
    }
    #endregion

    #region infamy
    public float CurrentInfamy = 0;
    public void IncreaseInfamy(float value)
    {
        CurrentInfamy += value;
    }
    #endregion
}
