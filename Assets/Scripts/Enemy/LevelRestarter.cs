using System;
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

    public bool RestartEnabled
    {
        set
        {
            restartUI.enabled = value;
            // UI hierarchy gets destroyed on enabled = false
            if (value)
            {
                restartAction.Enable();
                restartButton = restartUI.rootVisualElement.Q<Button>();
                restartButton.clicked += RestartButtonClicked;
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
    }

    void Start()
    {
        restartAction = InputSystem.actions.FindAction("Restart");
        restartAction.performed += OnRestartLevelAction;
        restartUI = GetComponent<UIDocument>();
        RestartEnabled = false;
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
