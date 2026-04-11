using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public partial class EnemyUI : MonoBehaviour
{
    public EnemyAIState State;

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

        if (State == EnemyAIState.Idle)
        {
            label.text = "";
        }
        else if (State == EnemyAIState.Investigating)
        {
            label.text = "?";
            color = Color.yellow;
            blinkingSpeed = 0;
        }
        else if (State == EnemyAIState.Searching)
        {
            label.text = "??";
            color = Color.orange;
            blinkingSpeed = 3;
        }
        else if (State == EnemyAIState.Alerted)
        {
            label.text = "!!";
            color = Color.red;
            blinkingSpeed = 10;
        }

        float alpha = (Mathf.Cos(Time.time * blinkingSpeed) + 1.0f) / 2.0f;
        label.style.color = new StyleColor(color.WithAlpha(alpha));
    }
}
