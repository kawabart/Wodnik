using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region hiding
    public bool Hidden
    {
        get
        {
            if (isPushing) return false;
            return visibilityController == null ? false : visibilityController.Hidden;
        }
    }

    private VisibilityController visibilityController;
    #endregion

    #region aiminig
    private enum AimingDevice
    {
        Mouse, Gamepad
    }

    private Vector2 prevMousePos;
    private AimingDevice currentAimingDevice = AimingDevice.Mouse;
    public GameObject Hair;
    public Vector3 AimDirection = Vector3.zero;

    private void UpdateAimDirection()
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
                AimDirection = new Vector3(gamepadDirection.x, 0, gamepadDirection.y);
        }
        else if (currentAimingDevice == AimingDevice.Mouse)
        {
            prevMousePos = mousePos;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Plane plane = new Plane(Vector3.up, transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 mouseWorld = ray.GetPoint(distance);
                AimDirection = mouseWorld - transform.position;
            }
        }
    }
    #endregion

    #region movement
    private Rigidbody rigidBody;
    private InputAction moveAction;
    private InputAction sprintAction;

    [SerializeField]
    private Animator animator;
    private Vector3 moveInput = Vector3.zero;

    public float SprintingSpeed = 4.0f;
    public float WalkingSpeed = 2.0f;
    public float Acceleration = 1;
    private bool sprintInput = false;

    private bool IsMovementLocked()
    {
        bool locked = false;
        if (!IsAlive) return true;
        if (isTakedown) return true;
        if (isPushing) return true;
        return locked;
    }

    private void UpdatePositionDirection()
    {
        if (IsMovementLocked()) moveInput = Vector3.zero;

        rigidBody.linearVelocity = Vector3.MoveTowards(rigidBody.linearVelocity, moveInput * (sprintInput ? SprintingSpeed : WalkingSpeed), Acceleration * Time.fixedDeltaTime);
        if (isTakedown)
        {
            TakedownUpdate();
        }
        else if (isPushing)
        {
            float angle = Mathf.Atan2(AimDirection.x, AimDirection.z) * Mathf.Rad2Deg;
            rigidBody.MoveRotation(Quaternion.Euler(0, angle, 0));
        }
        else if (!IsZero(rigidBody.linearVelocity))
        {
            float angle = Mathf.Atan2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.z) * Mathf.Rad2Deg;
            rigidBody.MoveRotation(Quaternion.Euler(0, angle, 0));
        }
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
            return IsAlive && Health < MaxHealth;
        }
    }

    public void Kill()
    {
        Health = 0;
        animator.SetBool("isDead", true);
        if (LevelRestarter.Instance != null)
        {
            LevelRestarter.Instance.IsRestartEnabled = true;
        }
    }
    public void TakeDamage(DamageData damageData)
    {
        if (!IsAlive) return;

        if (GetComponent<Surface>() != null)
            EffectSpawner.Instance.SpawnHit(transform.position, GetComponent<Surface>().type);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);

        Health = Math.Max(0, Health - Math.Max(0, damageData.Amount));
        if (!IsAlive) Kill();
    }

    public void Heal(int heal)
    {
        Health = Math.Min(MaxHealth, Health + Math.Max(0, heal));
    }
    #endregion

    #region push
    private InputAction push;
    public bool isPushing = false;
    public float pushForce = 10f;
    public float pushRadius = 0.2f;
    public float pushOffset = 0.2f;

    void OnPush(InputAction.CallbackContext ctx)
    {
        Push();
    }

    void Push()
    {
        if (IsMovementLocked()) return;
        isPushing = true;
        UpdateAimDirection();
        animator.SetTrigger("push");
        Debug.Log("Pushing starts...");
    }

    public void ApplyPushForce()
    {
        Debug.Log("Push!");
        Vector3 center = transform.position + transform.forward * pushOffset;
        Collider[] hits = Physics.OverlapSphere(center, pushRadius);
        Vector3 force = transform.forward * pushForce;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IPushable>(out var pushable))
            {
                pushable.Push(force);
            }
        }

    }

    public void EndPush()
    {
        isPushing = false;
        Debug.Log("Pushing ends.");
    }

    #endregion

    #region grab
    private HairController hairController;
    [SerializeField] private LayerMask grabMask;
    public float GrabDistance = 2;
    public float GrabAutoAim = .5f;
    private InputAction grab;
    public bool IsGrabbing = false;

    void OnGrab(InputAction.CallbackContext ctx)
    {
        if (IsGrabbing) return;
        Grab();
    }

    void OnGrabLetGo(InputAction.CallbackContext ctx)
    {
        if (!IsGrabbing) return;

        LetGo();
    }

    public Transform targetedGrabObject = null;
    Vector3 targetPoint = Vector3.zero;

    void GetGrabTarget()
    {
        UpdateAimDirection();
        RaycastHit raycastHit;
        Vector3 heightOffset = 0.2f * Vector3.up;
        if (Physics.Raycast(transform.position + heightOffset, AimDirection, out raycastHit, GrabDistance, grabMask))
        {
            targetPoint = raycastHit.point;
        }
        else if (Physics.Raycast(transform.position, AimDirection, out raycastHit, GrabDistance, grabMask))
        {
            targetPoint = raycastHit.point;
        }
        else if (Physics.Raycast(transform.position + heightOffset, Quaternion.Euler(0f, -5f, 0f) * AimDirection, out raycastHit, GrabDistance, grabMask))
        {
            targetPoint = raycastHit.point;
        }
        else if (Physics.Raycast(transform.position + heightOffset, Quaternion.Euler(0f, 5f, 0f) * AimDirection, out raycastHit, GrabDistance, grabMask))
        {
            targetPoint = raycastHit.point;
        }
        else
        {
            targetPoint = transform.position + AimDirection.normalized * GrabDistance;
        }

        Collider[] hits = Physics.OverlapSphere(targetPoint, GrabAutoAim);

        bool foundGrabbable = false;
        foreach (var hit in hits)
        {
            if (foundGrabbable) continue;
            if (hit.TryGetComponent<IGrabbable>(out var grabbable))
            {
                if (grabbable.CanBeGrabbed())
                {
                    foundGrabbable = true;
                    targetedGrabObject = hit.transform;
                }
            }
        }

        if (!foundGrabbable) targetedGrabObject = null;
    }

    void Grab()
    {
        Debug.Log("Grab!");
        hairController.Probe(targetPoint);
        if (targetedGrabObject == null) GetGrabTarget();
        if (targetedGrabObject == null) return;
        if (targetedGrabObject.GetComponent<IGrabbable>().Grab(hairController))
            IsGrabbing = true;

    }

    void LetGo()
    {
        Debug.Log("Grab ends.");
        hairController.LetGo();
        IsGrabbing = false;
    }
    #endregion

    #region takedown
    private InputAction takedown;
    public bool isTakedown = false;
    public Transform takedownTarget = null;
    public float takedownRadius = .5f;
    void OnTakedown(InputAction.CallbackContext ctx)
    {
        StartTakedown();
    }

    void StartTakedown()
    {
        if (IsMovementLocked()) return;
        if (!GetTakedownTarget()) return;

        isTakedown = true;
        animator.SetTrigger("takedown");
        Debug.Log("Takedown starts...");

    }
    bool GetTakedownTarget()
    {
        Vector3 center = transform.position;
        Collider[] hits = Physics.OverlapSphere(center, takedownRadius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<EnemyController>(out var enemy))
            {
                if (enemy.CurrentState == EnemyState.Downed)
                {
                    takedownTarget = enemy.transform;
                    enemy.TurnPhysicsOff();
                    return true;
                }
            }
        }
        return false;
    }
    void TakedownUpdate()
    {
        if (takedownTarget == null)
        {
            isTakedown = false;
            return;
        }
        Vector3 forwardDirection = takedownTarget.forward;
        Vector3 upDirection = Vector3.up;
        Quaternion targetRotation = Quaternion.LookRotation(-forwardDirection, upDirection);
        SmoothAlignToTarget(takedownTarget.transform.position - takedownTarget.transform.forward * .2f, targetRotation, .5f);
    }
    void SmoothAlignToTarget(Vector3 targetPosition, Quaternion targetRotation, float lerpFactor = 0.2f)
    {
        rigidBody.MovePosition(Vector3.Lerp(rigidBody.position, targetPosition, lerpFactor));
        rigidBody.MoveRotation(Quaternion.Slerp(rigidBody.rotation, targetRotation, lerpFactor));
    }
    public void KillTakedownTarget()
    {
        takedownTarget.GetComponent<IDamageable>().TakeDamage(new DamageData(10));
    }
    #endregion

    void Start()
    {
        moveAction.Enable();
        rigidBody = GetComponent<Rigidbody>();
        hairController = GetComponent<HairController>();
        if (animator == null)
            animator = GetComponent<Animator>();
        prevMousePos = Mouse.current != null ? Mouse.current.position.value : Vector2.zero;
        visibilityController = GetComponent<VisibilityController>();
        Health = MaxHealth;
    }

    private void Update()
    {
        moveInput = new Vector3(moveAction.ReadValue<Vector2>().x, 0, moveAction.ReadValue<Vector2>().y);
        sprintInput = sprintAction.ReadValue<float>() > 0.5f;
        animator.SetFloat("playerSpeed", rigidBody.linearVelocity.magnitude);
        animator.SetBool("hidden", Hidden);
        animator.SetBool("isPushing", isPushing);
        animator.SetBool("isTakedown", isTakedown);
        GetGrabTarget();
    }

    void FixedUpdate()
    {
        UpdatePositionDirection();
    }

    void OnEnable()
    {
        Debug.Log("Subscription");
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.Enable();
        push = InputSystem.actions.FindAction("Push");
        push.Enable();
        push.performed += OnPush;
        grab = InputSystem.actions.FindAction("Grab");
        grab.Enable();
        grab.performed += OnGrab;
        grab.canceled += OnGrabLetGo;
        takedown = InputSystem.actions.FindAction("Takedown");
        takedown.Enable();
        takedown.performed += OnTakedown;
    }

    void OnDisable()
    {
        push.performed -= OnPush;
        grab.performed -= OnGrab;
        grab.canceled -= OnGrabLetGo;
        takedown.performed -= OnTakedown;
    }

    private static bool IsZero(Vector2 v)
    {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f);
    }

    private static bool IsZero(Vector3 v)
    {
        return Mathf.Approximately(v.x, 0.0f) && Mathf.Approximately(v.y, 0.0f) && Mathf.Approximately(v.z, 0.0f);
    }
}
