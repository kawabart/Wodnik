using UnityEngine;

public class ToggleWeapon : MonoBehaviour
{
    public GameObject weapon;

    public void HideItem()
    {
        weapon?.SetActive(false);
    }

    public void ShowItem()
    {
        weapon?.SetActive(true);
    }
}
