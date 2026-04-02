using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region hiding
    public bool Hidden;
    private VisibilityController visibilityController;
    private bool CheckPlayerHiddenState()
    {
        if (visibilityController == null) return false;
        return visibilityController.Hidden;
    }
    #endregion

    #region aiminig
    private enum AimingDevice
    {
        Mouse, Gamepad
    }

    private Vector2 prevMousePos;
    private AimingDevice currentAimingDevice = AimingDevice.Mouse;
    public GameObject Hair;

    private void UpdateHairDirection()
    {
        Vector2 gamepadDirection = Gamepad.current != null ? Gamepad.current.rightStick.value : Vector2.zero;
        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.value : prevMousePos;

        if (prevMousePos != mousePos)
        {
            currentAimingDevice = AimingDevice.Mouse;
        }
        else if (!IsZero(gamepadDirection))
        {
            currentAimingDevice = AimingDevice.Gamepad;
        }

        if (currentAimingDevice == AimingDevice.Gamepad)
        {
            if (!IsZero(gamepadDirection))
                RotateTowards(Hair, new Vector3(gamepadDirection.x, 0, gamepadDirection.y));
        }
        else if (currentAimingDevice == AimingDevice.Mouse)
        {
            prevMousePos = mousePos;
            var mouseScreen = new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.y);
            var mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            RotateTowards(Hair, transform.position - mouseWorld);
        }
    }
    #endregion

    #region movement
    private Rigidbody rigidBody;
    public InputAction MoveAction;
    [SerializeField]
    private Animator animator;
    private Vector3 moveInput = Vector3.zero;

    public float MovementSpeed = 3.0f;
    public float Acceleration = 1;
    private void UpdatePositionDirection()
    {
        rigidBody.linearVelocity = Vector3.MoveTowards(rigidBody.linearVelocity, moveInput * MovementSpeed, Acceleration * Time.fixedDeltaTime);

        if (!IsZero(rigidBody.linearVelocity))
            RotateTowards(gameObject, rigidBody.linearVelocity);
    }
    #endregion

    #region health
    public int MaxHealth = 3;
    public int WoundedHealth = 1;
    public int Health;

    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    public bool IsWounded
    {
        get
        {
            return IsAlive && Health <= WoundedHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        Health = Math.Max(0, Health - Math.Max(0, damage));
    }

    public void Heal()
    {
        Health = MaxHealth;
    }
    #endregion

    void Start()
    {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponent<Animator>();
        prevMousePos = Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
        visibilityController = GetComponent<VisibilityController>();
        Health = MaxHealth;
    }

    private void Update()
    {
        moveInput = new Vector3(MoveAction.ReadValue<Vector2>().x, 0, MoveAction.ReadValue<Vector2>().y);
        animator.SetFloat("playerSpeed", rigidBody.linearVelocity.magnitude);
        animator.SetBool("hidden", Hidden);
        UpdateHairDirection();
    }

    void FixedUpdate()
    {
        if (!IsAlive) return;
        UpdatePositionDirection();
        Hidden = CheckPlayerHiddenState();
    }

    private static bool IsZero(Vector2 v)
    {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }
    private static bool IsZero(Vector3 v)
    {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f) && Mathf.Approximately(v.z, 0.0f);
    }

    /**
     * Rotating game objects to face certain direction using transform.up
     * leads to issues with quaternion rotation, hence this helper.
     **/
    private static void RotateTowards(GameObject obj, Vector3 v)
    {
        float angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
