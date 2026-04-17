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
            var mouseScreen = new Vector3(mousePos.x, mousePos.y, 0);
           
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
    public InputAction MoveAction;
    [SerializeField]
    private Animator animator;
    private Vector3 moveInput = Vector3.zero;

    public float MovementSpeed = 3.0f;
    public float Acceleration = 1;
    private bool IsMovementLocked()
    {
        bool locked = false;
        if (!IsAlive) return true;
        if (isPushing) return true;
        return locked;
    }
    private void UpdatePositionDirection()
    {
        if (IsMovementLocked()) moveInput =Vector3.zero;
        rigidBody.linearVelocity = Vector3.MoveTowards(rigidBody.linearVelocity, moveInput * MovementSpeed, Acceleration * Time.fixedDeltaTime);
        if (isPushing)
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
        animator.SetBool("isDead",true);
    }
    public void TakeDamage(int damage, GameObject source)
    {
        if (!IsAlive) return;

        if (GetComponent<Surface>() != null)
            EffectSpawner.Instance.SpawnHit(transform.position, GetComponent<Surface>().type);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);

        Health = Math.Max(0, Health - Math.Max(0, damage));
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
    void Grab()
    {
        Debug.Log("Grab!");
        UpdateAimDirection();
        Vector3 targetPoint;
        if (Physics.Raycast(transform.position+.2f*Vector3.up, AimDirection, out RaycastHit raycastHit, GrabDistance, grabMask)) 
            targetPoint = raycastHit.point;
        else 
            targetPoint = transform.position + AimDirection.normalized * GrabDistance;

        Collider[] hits = Physics.OverlapSphere(raycastHit.point, GrabAutoAim);
        hairController.Probe(targetPoint);
        bool foundGrabbable = false;
        foreach (var hit in hits)
        {
            if (foundGrabbable) continue;
            if (hit.TryGetComponent<IGrabbable>(out var grabbable))
            {
                if (grabbable.Grab(hairController))
                    foundGrabbable = true;
            }
        }
        IsGrabbing = true;
    }
    void LetGo()
    {
        Debug.Log("Grab ends.");
        hairController.LetGo();
        IsGrabbing = false;
    }
    #endregion
    void Start()
    {
        MoveAction.Enable();
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
        moveInput = new Vector3(MoveAction.ReadValue<Vector2>().x, 0, MoveAction.ReadValue<Vector2>().y);
        animator.SetFloat("playerSpeed", rigidBody.linearVelocity.magnitude);
        animator.SetBool("hidden", Hidden);
        animator.SetBool("isPushing", isPushing);
        UpdateAimDirection();
    }

    void FixedUpdate()
    {
        UpdatePositionDirection();
    }
    void OnEnable()
    {
        Debug.Log("Subscription");
        push = InputSystem.actions.FindAction("Push");
        push.Enable();
        push.performed += OnPush;
        grab = InputSystem.actions.FindAction("Grab");
        grab.Enable();
        grab.performed += OnGrab;
        grab.canceled += OnGrabLetGo;
    }

    void OnDisable()
    {
        push.performed -= OnPush;
        grab.performed -= OnGrab;
        grab.canceled -= OnGrabLetGo;
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
