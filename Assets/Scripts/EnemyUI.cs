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
    public AgitationController agitationController;
    public EnemyController enemyController;
    void Start()
    {
        document = GetComponent<UIDocument>();
        label = document.rootVisualElement.Q<Label>();
        agitationController = GetComponentInParent<AgitationController>();
        enemyController = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        if (enemyController.CurrentState!= EnemyState.Alive)
        {
            label.text = "";
            return;
        }

        var angle = transform.parent.localEulerAngles.y;
        transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(transform.localEulerAngles.x, 0, angle));

        if (State == EnemyAIState.Idle)
        {
            label.text = "...";
            blinkingSpeed = 0;
        }
        else if (State == EnemyAIState.Investigating)
        {
            label.text = "?";
            blinkingSpeed = 0;
        }
        else if (State == EnemyAIState.Searching)
        {
            label.text = "!?";
            blinkingSpeed = 3;
        }
        else if (State == EnemyAIState.Alerted)
        {
            label.text = "!";
            blinkingSpeed = 10;
        }

        if (agitationController.AgitationState == AgitationState.Relaxed) color = Color.white;
        if (agitationController.AgitationState == AgitationState.Investigating) color = Color.yellow;
        if (agitationController.AgitationState == AgitationState.Alarmed) color = Color.red;
        //blinkingSpeed = agitationController.AgitationLevel/5;
        float alpha = agitationController.AgitationLevel / 100;//(Mathf.Cos(Time.time * blinkingSpeed) + 3f) / 4f;
        label.style.color = new StyleColor(color.WithAlpha(alpha));
    }
}
