using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private enum AimingDevice {
        Mouse, Gamepad
    }

    private AimingDevice currentAimingDevice = AimingDevice.Mouse;
    public GameObject Hair;

    private Rigidbody2D rigidBody;

    public InputAction MoveAction;
    private Vector2 prevMousePos;

    public float MovementSpeed = 3.0f;

    void Start() {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
        prevMousePos = Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
    }

    void FixedUpdate() {
        UpdatePositionDirection();
        UpdateHairDirection();
    }

    private void UpdatePositionDirection() {
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();
        if (!IsZero(moveInput)) {
            Vector2 position = rigidBody.position + moveInput * MovementSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(position);
            transform.up = moveInput;
        }
    }

    private bool IsZero(Vector2 v) {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }

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
        }
    }
}
