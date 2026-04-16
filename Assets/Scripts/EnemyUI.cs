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
    public EnemyPerception enemyPerception;
    void Start()
    {
        document = GetComponent<UIDocument>();
        label = document.rootVisualElement.Q<Label>();
        agitationController = GetComponentInParent<AgitationController>();
        enemyController = GetComponentInParent<EnemyController>();
        enemyPerception = GetComponentInParent<EnemyPerception>();
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

        if (enemyPerception.PerceptionState == EnemyPerceptionState.Idle)
        {
            label.text = "...";
        }
        else if (enemyPerception.PerceptionState == EnemyPerceptionState.PlayerSeenRecently)
        {
            label.text = "?";
        }
        else if (enemyPerception.PerceptionState == EnemyPerceptionState.PlayerInSight)
        {
            label.text = "!";
        }
        
        if (agitationController.AgitationState == AgitationState.Relaxed) color = Color.white;
        if (agitationController.AgitationState == AgitationState.Investigating) color = Color.yellow;
        if (agitationController.AgitationState == AgitationState.Alarmed) color = Color.red;

        float alpha = agitationController.AgitationLevel / 100;
        label.style.color = new StyleColor(color.WithAlpha(alpha));
    }
}
