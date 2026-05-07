using UnityEngine;

public class LevelController : MonoBehaviour
{
    public void CompleteLevel()
    {
        GameManager.Instance.CompleteLevel();
    }
}
