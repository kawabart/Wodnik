using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    //Nonbinary inputs, that only affect gameplay when using GetLightValueOnObject function. For now we only use binary IsInLight function.
    [SerializeField] 
    private float brightness;
    [SerializeField, Tooltip("Affects value of light over distance.")]
    private AnimationCurve lightCurve;
    [SerializeField, Tooltip("Multiplyes default brightness value of Light component.")]
    private float lightBrightnessOverride = 0.5f;

    private LayerMask shadowCastingLayerMask; //layers that block light in gameplay
    private LayerMask playerMask; //player's layer

    void Start()
    {
        lightComponent = GetComponent<Light>();
        shadowCastingLayerMask = LayerMask.GetMask("Blockout");
        playerMask = LayerMask.GetMask("Player");
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

    private bool IsInDistance(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= range;
    }

    public bool IsInLight(Transform target)
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
    private bool IsObscured(Transform target)
    {
        Vector3 direction = target.position + Vector3.up * 0.05f - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range, playerMask))
            if (hit.transform == target)
                return false;
        return true;
    }
    private bool IsAngleRight(Transform target)
    {
        float viewAngle = lightComponent.spotAngle / 2;
        Vector3 direction = target.position - transform.position + Vector3.up * .05f;
        float angle = Vector3.Angle(transform.forward, direction);

        Color rayColor = Color.white;
        if (angle > viewAngle)
            Debug.DrawRay(transform.position, direction * 10, Color.red);
        else
            Debug.DrawRay(transform.position, direction * 10, Color.yellow);

        if (angle > viewAngle) return false;
        return true;
    }
    private bool IsObscuredDirection(Transform target)
    {
        Vector3 direction = -lightComponent.transform.forward;
        RaycastHit hit;
        Debug.DrawRay(target.position + Vector3.up, direction * 100, Color.yellow, 1f);
        Vector3 point1 = target.position + Vector3.forward * .05f + Vector3.up * .05f;
        Vector3 point2 = target.position - Vector3.forward * .05f + Vector3.up * .05f;
        if (!Physics.Raycast(point1, direction, out hit, 150, shadowCastingLayerMask))
            return false;
        if (!Physics.Raycast(point2, direction, out hit, 150, shadowCastingLayerMask))
            return false;
        return true;
    }
    public float GetLightValueOnObject(Transform target)
    {
        float lightValue;
        float distance = Vector3.Distance(transform.position, target.position);
        float lerpValue = 1 - distance / range;
        lerpValue = lightCurve.Evaluate(lerpValue);
        lightValue = Mathf.Lerp(0, brightness, lerpValue);
        return lightValue;
    }

    public void KillLight()
    {
        lightComponent.enabled = false;
    }
}
