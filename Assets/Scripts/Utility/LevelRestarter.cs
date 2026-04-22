using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LevelRestarter : MonoBehaviour
{
    public static LevelRestarter Instance;

    private InputAction restartAction;
    private UIDocument restartUI;
    private Button restartButton;

    public bool IsRestartEnabled
    {
        set
        {
            restartUI.rootVisualElement.visible = value;
            if (value)
            {
                restartAction.Enable();
            }
            else
            {
                restartAction.Disable();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        restartAction = InputSystem.actions.FindAction("Restart");
        restartUI = GetComponent<UIDocument>();
    }

    void Start()
    {
        IsRestartEnabled = false;
    }

    private void OnEnable()
    {
        restartAction.performed += OnRestartLevelAction;
        restartButton = restartUI.rootVisualElement.Q<Button>();
        restartButton.clicked += RestartButtonClicked;
    }

    private void OnDisable()
    {
        restartAction.performed -= OnRestartLevelAction;
        restartButton.clicked -= RestartButtonClicked;
    }

    private void RestartButtonClicked()
    {
        RestartLevel();
    }

    private void OnRestartLevelAction(InputAction.CallbackContext obj)
    {
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
