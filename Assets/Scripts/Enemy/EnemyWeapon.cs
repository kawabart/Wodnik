using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    public int damage = 1;
    public LayerMask mask;
    public bool isAttacking = false;

    public WeaponScriptable weaponScriptable = null;
    [SerializeField] private WeaponPoints weaponInHands = null;

    public float radius = 0.5f;

    [SerializeField] private Transform weaponSlot;

    private void Start()
    {
        if (weaponScriptable != null && weaponInHands == null) EquipWeapon(weaponScriptable);
    }
    public void EquipWeapon(WeaponScriptable scriptable)
    {
        if (weaponInHands != null) DropWeapon();

        weaponScriptable = scriptable;
        weaponInHands = Instantiate(weaponScriptable.enemyWeaponPrefab.gameObject, weaponSlot).GetComponent<WeaponPoints>();

    }
    public void StartAttack()
    {
        isAttacking = true;
    }
    public void DoImpulseAttack()
    {
        Debug.Log("Attack casts damage!");
        if (weaponInHands == null) return;

        Collider[] hits = Physics.OverlapCapsule(
            weaponInHands.PointA.position,
            weaponInHands.PointB.position,
            radius,
            mask
        );

        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(new DamageData(weaponScriptable.damage));
            }
        }
        isAttacking = false;
    }

    public void FinishAttack()
    {
        Debug.Log("Attack Finished.");
        isAttacking = false;
    }

    public void DropWeapon()
    {
        if (weaponInHands != null) Destroy(weaponInHands.gameObject);
        if (weaponScriptable == null) return;
        if (weaponScriptable.onGroundPrefab == null) return;

        Vector3 defaultRotation = weaponScriptable.onGroundPrefab.transform.eulerAngles;
        float slotY = weaponSlot.transform.eulerAngles.y;
        Quaternion finalRotation = Quaternion.Euler(defaultRotation.x, slotY, defaultRotation.z);

        Instantiate(weaponScriptable.onGroundPrefab, weaponSlot.transform.position, finalRotation);
    }
}
