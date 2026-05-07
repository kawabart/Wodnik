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
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("GameManager initialized");
    }

    public int CurrentLevel = 0;
    public int CurrentChaos = 0;
}
