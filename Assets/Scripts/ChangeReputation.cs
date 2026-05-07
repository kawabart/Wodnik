using UnityEngine;

public class ChangeReputation : MonoBehaviour
{
    public void IncreaseChaos(float value)
    {
        GameManager.Instance.IncreaseChaos(value);
    }
}
