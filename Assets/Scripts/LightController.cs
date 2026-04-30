using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour
{
    [SerializeField]
    private Light lightComponent;
    [SerializeField]
    private LightType lightType;
    [SerializeField]
    private float range;

    [SerializeField, Tooltip("Cuts off slightly the range of LightController compared to Light component, so that there's some visual feedback before you're considered 'lit' by the game.")]
    private float lightRangeOverride = 0.8f;
    [SerializeField]
    private bool isOn = true;
    [SerializeField, Tooltip("If true, it updates in realtime during gameplay. If false, it only reads values once on start.")]
    private bool realtime = false;
    [SerializeField, Tooltip("How far from target in the direction of directional light should we check for shadow casters.")]
    private float directionalShadowCheckLength = 50;

    //Nonbinary inputs, that only affect gameplay when using GetLightValueOnPoint function. For now we only use binary IsInLight function.
    [SerializeField]
    private float brightness;
    [SerializeField, Tooltip("Affects value of light over distance.")]
    private AnimationCurve lightCurve;
    [SerializeField, Tooltip("Multiplyes default brightness value of Light component.")]
    private float lightBrightnessOverride = 0.5f;

    private LayerMask shadowCastingLayerMask; //layers that block light in gameplay

    void Start()
    {
        lightComponent = GetComponent<Light>();
        shadowCastingLayerMask = LayerMask.GetMask("Blockout", "Dynamic");
        UpdateLight();
    }
    void UpdateLight()
    {
        lightType = lightComponent.type;
        if (lightType == LightType.Directional)
            range = Mathf.Infinity;
        else
            range = lightComponent.range * lightRangeOverride;
        brightness = lightComponent.intensity * lightBrightnessOverride;

    }
    private void Update()
    {
        if (realtime) UpdateLight();
    }

    public bool IsInLight(Vector3 target)
    {
        if (lightComponent == null) return false;
        if (!isOn) return false;

        switch (lightComponent.type)
        {
            case LightType.Point:
                if (!IsInDistance(target)) return false;
                if (IsObscured(target)) return false;
                break;
            case LightType.Directional:
                if (IsObscuredDirection(target)) return false;
                break;
            case LightType.Spot:
                if (!IsInDistance(target)) return false;
                if (!IsAngleRight(target)) return false;
                if (IsObscured(target)) return false;
                break;
        }
        return true;
    }
    private bool IsInDistance(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= range;
    }
    private bool IsObscured(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        RaycastHit hit;
        if (!Physics.Raycast(target, direction, out hit, direction.magnitude, shadowCastingLayerMask))
            return false;
        return true;
    }
    private bool IsAngleRight(Vector3 target)
    {
        float viewAngle = lightComponent.spotAngle / 2;
        Vector3 direction = target - transform.position + Vector3.up * .05f;
        float angle = Vector3.Angle(transform.forward, direction);

        Color rayColor = Color.white;
        if (angle > viewAngle)
            Debug.DrawRay(transform.position, direction * 10, Color.red);
        else
            Debug.DrawRay(transform.position, direction * 10, Color.yellow);

        if (angle > viewAngle) return false;
        return true;
    }
    private bool IsObscuredDirection(Vector3 target)
    {
        Vector3 direction = -lightComponent.transform.forward;
        RaycastHit hit;


        if (!Physics.Raycast(target, direction, out hit, directionalShadowCheckLength, shadowCastingLayerMask))
        {
            Debug.DrawRay(target, direction * directionalShadowCheckLength, Color.yellow, .1f);
            return false;
        }
        Debug.DrawLine(target, hit.point, Color.gray, .1f);
        return true;
    }
    public float GetLightValueOnPoint(Vector3 target)
    {
        float lightValue;
        float distance = Vector3.Distance(transform.position, target);
        float lerpValue = 1 - distance / range;
        lerpValue = lightCurve.Evaluate(lerpValue);
        lightValue = Mathf.Lerp(0, brightness, lerpValue);
        return lightValue;
    }

    public void KillLight()
    {
        isOn = false;
        lightComponent.enabled = false;
    }
}
