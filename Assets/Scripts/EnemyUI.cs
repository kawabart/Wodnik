using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EnemyUI : MonoBehaviour
{
    public enum AIState
    {
        Idle, Investigating, Searching, Alerted
    }

    public AIState State;

    private UIDocument document;
    private Label label;

    private Color color = Color.white;
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
            color = Color.yellow;
            blinkingSpeed = 0;
        }
        else if (State == AIState.Searching)
        {
            label.text = "??";
            color = Color.orange;
            blinkingSpeed = 3;
        }
        else if (State == AIState.Alerted)
        {
            label.text = "!!";
            color = Color.red;
            blinkingSpeed = 10;
        }

        float alpha = (Mathf.Cos(Time.time * blinkingSpeed) + 1.0f) / 2.0f;
        label.style.color = new StyleColor(color.WithAlpha(alpha));
    }
}
