using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public partial class EnemyUI : MonoBehaviour
{
    public EnemyAIState State;

    private UIDocument document;
    private Label label;

    private Color color = Color.white;
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
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        if (enemyController.CurrentState == EnemyState.Downed)
        {
            label.text = "zzZ";
            return;
        }
        else if (enemyController.CurrentState != EnemyState.Alive)
        {
            label.text = "";
            return;
        }
        if (agitationController.IsShocked())
            label.text = "?!";
        else if (enemyPerception.PerceptionState == EnemyPerceptionState.Idle)
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
        label.style.color = new StyleColor(new Color(color.r, color.g, color.b, alpha));
    }
}
