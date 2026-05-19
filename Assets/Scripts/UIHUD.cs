using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIHUD : MonoBehaviour
{
    private UIDocument document;
    private Label label;

    [Header("Animation")]
    [SerializeField] private float scaleBoost = 0.25f;
    [SerializeField] private float lerpSpeed = 6f;
    [SerializeField] private Color increaseColor = new Color(1f, 0.35f, 0.35f);
    [SerializeField] private Color decreaseColor = new Color(0.35f, 1f, 0.35f);

    private float shownInfamy = 0;

    private Color defaultColor;
    private float targetScale = 1f;
    private float currentScale = 1f;
    private Color currentColor;

    void Start()
    {
        document = GetComponent<UIDocument>();
        label = document.rootVisualElement.Q<Label>();

        defaultColor = label.resolvedStyle.color;
        currentColor = defaultColor;

        UpdateVisualsInstant();
    }

    void Update()
    {
        UpdateInfamy();
        AnimateLabel();
    }

    #region infamy

    void UpdateInfamy()
    {
        float currentInfamy = GameManager.Instance.CurrentInfamy;

        if (Mathf.Approximately(currentInfamy, shownInfamy))
            return;

        bool increased = currentInfamy > shownInfamy;

        shownInfamy = currentInfamy;

        label.text = "Infamy: " + shownInfamy;

        // punch scale
        currentScale = 1f + scaleBoost;

        // flash color
        currentColor = increased ? increaseColor : decreaseColor;
    }

    void AnimateLabel()
    {
        // scale fade
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * lerpSpeed);

        // color fade
        currentColor = Color.Lerp(currentColor, defaultColor, Time.deltaTime * lerpSpeed);

        label.style.scale = new Scale(new Vector2(currentScale, currentScale));
        label.style.color = new StyleColor(currentColor);
    }

    void UpdateVisualsInstant()
    {
        label.text = "Infamy: " + shownInfamy;
        label.style.scale = new Scale(Vector2.one);
        label.style.color = new StyleColor(defaultColor);
    }

    #endregion
}
