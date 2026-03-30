using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    #region aiminig
    private enum AimingDevice {
        Mouse, Gamepad
    }

    private Vector2 prevMousePos;
    private AimingDevice currentAimingDevice = AimingDevice.Mouse;
    public GameObject Hair;

    private void UpdateHairDirection() {
        Vector2 gamepadDirection = Gamepad.current != null ? Gamepad.current.rightStick.value : Vector2.zero;
        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.value : prevMousePos;

        if (prevMousePos != mousePos) {
            currentAimingDevice = AimingDevice.Mouse;
        } else if (!IsZero(gamepadDirection)) {
            currentAimingDevice = AimingDevice.Gamepad;
        }

        if (currentAimingDevice == AimingDevice.Gamepad) {
            if (!IsZero(gamepadDirection))
                RotateTowards(Hair, -gamepadDirection);
        } else if (currentAimingDevice == AimingDevice.Mouse) {
            prevMousePos = mousePos;
            var mouseScreen = new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
            var mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            RotateTowards(Hair, transform.position - mouseWorld);
        }
    }
    #endregion

    #region movement
    private Rigidbody2D rigidBody;
    public InputAction MoveAction;
    [SerializeField] 
    private Animator animator;
    public float MovementSpeed = 3.0f;

    private void UpdatePositionDirection() {
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();
        if (!IsZero(moveInput)) {
            Vector2 position = rigidBody.position + moveInput * MovementSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(position);
            RotateTowards(gameObject, moveInput);
        }
        animator.SetFloat("playerSpeed", moveInput.magnitude * MovementSpeed);
    }
    #endregion

    #region health
    public int MaxHealth = 3;
    public int WoundedHealth = 1;
    public int Health;

    public bool IsAlive {
        get {
            return Health > 0;
        }
    }

    public bool IsWounded {
        get {
            return IsAlive && Health <= WoundedHealth;
        }
    }

    public void TakeDamage(int damage) {
        Health = Math.Max(0, Health - Math.Max(0, damage));
    }

    public void Heal() {
        Health = MaxHealth;
    }
    #endregion

    void Start() {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
        if (animator==null)
            animator = GetComponent<Animator>();
        prevMousePos = Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
        Health = MaxHealth;
    }

    void FixedUpdate() {
        if (IsAlive) {
            UpdatePositionDirection();
            UpdateHairDirection();
        }
    }

    private static bool IsZero(Vector2 v) {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }

    /**
     * Rotating game objects to face certain direction using transform.up
     * leads to issues with quaternion rotation, hence this helper.
     **/
    private static void RotateTowards(GameObject obj, Vector2 v) {
        float angle = -Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
