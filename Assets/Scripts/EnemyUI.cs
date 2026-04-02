using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(UIDocument))]
public class EnemyUI : MonoBehaviour
{
    public enum AIState
    {
        Idle, Investigating, Alerted
    }

    public AIState State;

    private UIDocument document;
    private Label label;

    private float blinkingSpeed = 5;

    void Start()
    {
        document = GetComponent<UIDocument>();
        label = document.rootVisualElement.Q<Label>();
    }

    void Update()
    {
        var angle = transform.parent.localEulerAngles.y;
        transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(transform.localEulerAngles.x, 0, angle));

        if (State == AIState.Idle)
        {
            label.text = "";
        }
        else if (State == AIState.Investigating)
        {
            label.text = "?";
            blinkingSpeed = 3;
        }
        else if (State == AIState.Alerted)
        {
            label.text = "!!";
            blinkingSpeed = 10;
        }

        float alpha = (Mathf.Sin(Time.time * blinkingSpeed) + 1.0f) / 2.0f;
        label.style.color = new StyleColor(label.resolvedStyle.color.WithAlpha(alpha));
    }
}
