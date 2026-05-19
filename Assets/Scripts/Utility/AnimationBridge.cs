using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
    [Tooltip("Drag the parent object (Enemy) in here")]
    public EnemyWeapon parentDamageDealer;

    public void PassDamageEvent()
    {
        if (parentDamageDealer != null)
        {
            parentDamageDealer.DoImpulseAttack();
        }
        else
        {
            Debug.LogWarning("AnimationBridge is missing the parent MeleeDamageDealer reference!");
        }
    }

    public void PassFinishAttackEvent()
    {
        if (parentDamageDealer != null)
        {
            parentDamageDealer.FinishAttack();
        }
    }
}
