using UnityEngine;

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
    }

    public int CurrentLevel = 0;
    #region chaos
    public float CurrentChaos = 0;
    public void IncreaseChaos(float value)
    {
        CurrentChaos += value;
    }
    #endregion
}
