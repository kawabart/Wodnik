using UnityEngine;

public class ChangeInfamy : MonoBehaviour
{
    public void IncreaseInfamy(int value)
    {
        GameManager.Instance.IncreaseInfamy(value);
    }
}

