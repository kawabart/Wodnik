using Assets.Player;
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
                Hair.transform.up = -gamepadDirection;
        } else if (currentAimingDevice == AimingDevice.Mouse) {
            prevMousePos = mousePos;
            var mouseScreen = new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z);
            var mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            Hair.transform.up = transform.position - mouseWorld;
            Vector3 euler = Hair.transform.eulerAngles;
            //little ovverride, so that rotation isn't applied to another axis
            Hair.transform.rotation = Quaternion.Euler(0f, 0f, euler.z);
        }
    }
    #endregion

    #region movement
    private Rigidbody2D rigidBody;
    public InputAction MoveAction;
    private Animator animator;
    public float MovementSpeed = 3.0f;

    private void UpdatePositionDirection() {
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();
        if (!IsZero(moveInput)) {
            Vector2 position = rigidBody.position + moveInput * MovementSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(position);
            transform.up = moveInput;
            Vector3 euler = transform.eulerAngles;
            //little ovverride, so that rotation isn't applied to another axis
            transform.rotation = Quaternion.Euler(0f, 0f, euler.z);
        }
        animator.SetFloat("playerSpeed", moveInput.magnitude*MovementSpeed);
    }
    #endregion

    #region health
    private HealthComponent health;

    public bool IsAlive => health.IsAlive;

    public bool IsWounded => health.IsWounded;

    public void TakeDamage(int damage) => health.TakeDamage(damage);

    public void Heal() => health.Heal();

    public void InitHealth() => health.InitHealth();
    #endregion

    public void Start() {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthComponent>();
        prevMousePos = Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
        InitHealth();
    }

    public void FixedUpdate() {
        if (IsAlive) {
            UpdatePositionDirection();
            UpdateHairDirection();
        }
    }

    private bool IsZero(Vector2 v) {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }
}
