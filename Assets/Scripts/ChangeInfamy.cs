using UnityEngine;

public class ChangeInfamy : MonoBehaviour
{
    public void IncreaseInfamy(float value)
    {
        GameManager.Instance.IncreaseInfamy(value);
    }
}

