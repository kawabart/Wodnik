using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private enum AimDevice {
        Mouse, Gamepad
    }

    private AimDevice currentAimDevice = AimDevice.Mouse;

    private Rigidbody2D rigidBody;

    public InputAction MoveAction;
    private Vector2 prevMousePos;

    public float MovementSpeed = 3.0f;

    void Start() {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
        prevMousePos = Mouse.current.position.value;
    }

    void FixedUpdate() {
        UpdatePosition();
        UpdateDirection();
    }

    private void UpdatePosition() {
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();
        if (!IsZero(moveInput)) {
            Vector2 position = rigidBody.position + moveInput * MovementSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(position);
        }
    }

    private bool IsZero(Vector2 v) {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }

    private void UpdateDirection() {
        Vector2 gamepadDirection = Gamepad.current.rightStick.value;
        if (prevMousePos != Mouse.current.position.value) {
            currentAimDevice = AimDevice.Mouse;
        } else if (!IsZero(gamepadDirection)) {
            currentAimDevice = AimDevice.Gamepad;
        }

        if (currentAimDevice == AimDevice.Gamepad) {
            if (!IsZero(gamepadDirection))
                transform.up = gamepadDirection;
        } else if (currentAimDevice == AimDevice.Mouse) {
            prevMousePos = Mouse.current.position.value;
            var mouseScreen = new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, -Camera.main.transform.position.z);
            var mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            transform.up = mouseWorld - transform.position;
        }
    }
}
