using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIHUD : MonoBehaviour
{
    private UIDocument document;
    private Label label;
    void Start()
    {
        document = GetComponent<UIDocument>();
        label = document.rootVisualElement.Q<Label>();
    }

    void Update()
    {
        label.text = "Chaos: "+GameManager.Instance.CurrentChaos;
    }
}
