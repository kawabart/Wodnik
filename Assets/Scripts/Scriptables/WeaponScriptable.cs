using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptable", menuName = "Scriptable Objects/WeaponScriptable")]
public class WeaponScriptable : ScriptableObject
{
    public string weaponName;

    public GameObject onGroundPrefab;
    public WeaponPoints enemyWeaponPrefab;

    public int damage;
}
