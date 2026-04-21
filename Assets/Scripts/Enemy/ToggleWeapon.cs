using UnityEngine;

public class ToggleWeapon : MonoBehaviour
{
    public GameObject weapon;

    // Those two methods are activated in animation events of certain enemy animations (eg. laying down or getting up)
    public void HideItem()
    {
       // if (weapon != null) weapon.SetActive(false);
    }

    public void ShowItem()
    {
        //if (weapon != null) weapon.SetActive(true);
    }
}
