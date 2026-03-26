using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rigidBody;

    public InputAction MoveAction;

    public float MovementSpeed = 3.0f;

    void Start() {
        MoveAction.Enable();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector2 moveInput = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f)) {
            Vector2 position = rigidBody.position + moveInput * MovementSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(position);

            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
