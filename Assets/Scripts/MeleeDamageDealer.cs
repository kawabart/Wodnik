using UnityEngine;

public class MeleeDamageDealer : MonoBehaviour
{
    public int damage = 1;
    public LayerMask mask;

    [Header("Capsule")]
    public Transform pointA;
    public Transform pointB;
    public float radius = 0.5f;

    [Header("Temporary stuff for visual feedback")]
    public GameObject weaponAttack;
    public GameObject weaponRest;
    public bool impulse = false;
    private float timer = 0;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            weaponAttack.SetActive(false);
            weaponRest.SetActive(true);
        }
        if (impulse) DoImpulseAttack();
    }
    public bool DoImpulseAttack()
    {
        //temporary
        weaponAttack.SetActive(true);
        weaponRest.SetActive(false);
        timer = .3f;
        impulse = false;
        //

        Collider[] hits = Physics.OverlapCapsule(
            pointA.position,
            pointB.position,
            radius,
            mask
        );

        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage, gameObject);
                //can be expanded in the future
                /*
                dmg.TakeDamage(new DamageData
                {
                    amount = damage,
                    point = hit.ClosestPoint(pointA.position),
                    direction = (hit.transform.position - transform.position).normalized,
                    source = gameObject
                });
                */
            }
        }
        return true;
    }
}
