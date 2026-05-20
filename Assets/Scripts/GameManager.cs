using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController CurrentPlayer = null;
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
        LoadPreviousLevelData();
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
        SaveManager.SaveLevelResult(CurrentLevel, CurrentInfamy);
        CurrentLevel++;
        if (CurrentLevel >= LevelNames.Length)
        {
            Debug.Log("CONGRATS! You won! Game Over (in a good way). Achievement unlocked: Happily ever after...");
            CurrentLevel = 0;
        }
         
            
            LoadLevel(CurrentLevel);
    
      
    }
    public void LoadLevel(int index)
    {
        LoadPreviousLevelData();
        SceneManager.LoadScene(LevelNames[CurrentLevel]);
        
    }
    public void RestartLevel()
    {
        LoadLevel(CurrentLevel);
    }

    #endregion

    #region infamy

    public int CurrentInfamy = 0;
    public int DefaultInfamy = 15;
    public void IncreaseInfamy(int value)
    {
        CurrentInfamy = Mathf.Max(0, Mathf.Min(100, CurrentInfamy + value));
    }
    #endregion

    #region save and load
    private void LoadPreviousLevelData()
    {
        Debug.Log("Trying to load save data...");
        int previousLevel = CurrentLevel - 1;

        if (previousLevel <= 0)
        {
            CurrentInfamy = DefaultInfamy;

            Debug.Log("No previous level save");
            return;
        }

        if (SaveManager.HasLevelResult(previousLevel))
        {
            CurrentInfamy =
                SaveManager.GetLevelInfamy(previousLevel);

            Debug.Log("Loaded previous level result");
        }
        else
        {
            CurrentInfamy = DefaultInfamy;
            Debug.Log("Using default values");
        }
    }

    #endregion
}
