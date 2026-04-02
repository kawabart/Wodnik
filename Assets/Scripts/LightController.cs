using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LightController : MonoBehaviour
{
    [SerializeField]
    private Light lightComponent;
    [SerializeField]
    private LightType lightType;
    [SerializeField]
    private float range;
    [SerializeField]
    private float brightness;
    [SerializeField]
    private AnimationCurve lightCurve;
    [SerializeField]
    private bool isOn = true;
    [SerializeField]
    private bool realtime = false;

    private LayerMask shadowCastingLayerMask;
    private LayerMask playerMask;



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
            range = lightComponent.range * 0.8f;
        brightness = lightComponent.intensity / 2;

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
