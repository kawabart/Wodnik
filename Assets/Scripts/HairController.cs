using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField] HairGenerator hairGenerator;
    private PlayerController playerController;
    private Rigidbody rigidBody;
    [SerializeField, Tooltip("Default position that hair end lerps to when it isn't attached to anything.")]
    private Transform defaultHairPosition;
    [SerializeField, Tooltip("Rigidbody of object that's currently grabbed by the hair. When null, hair returns to their default position. Drag and drop rigidbody to attach it.")]
    public Rigidbody GrabbedRb;
    public bool Grabbed = false;
    [SerializeField, Tooltip("Spring joint thats dynamically created (and removed) to attach rb thats grabbed by hair.")]
    private SpringJoint springJoint;
    public void Probe(Vector3 probeLocation)
    {
        hairGenerator.endpoint.transform.position = probeLocation;
    }
    public void Grab(Rigidbody rb)
    {
        GrabbedRb = rb;
        hairGenerator.endpoint.GetComponent<Follower>().Target = rb.transform;
        hairGenerator.endpoint.GetComponent<Follower>().SmoothTime = 0f;
        hairGenerator.noiseAmplitude = .1f;
        springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.minDistance = .5f;
        springJoint.connectedMassScale = 5;
        hairGenerator.maxDistance = 1;
        //to do: endsize can change based on dimensions of grabbed object.
        var cols = GrabbedRb.GetComponentsInChildren<Collider>();

        Bounds bounds = cols[0].bounds;
        for (int i = 1; i < cols.Length; i++)
        {
            bounds.Encapsulate(cols[i].bounds);
        }

        float minObjectSize = Mathf.Min(bounds.size.x, bounds.size.y, bounds.size.z);
        hairGenerator.endSize = minObjectSize;
        Grabbed = true;
    }

    public void LetGo()
    {
        if (GrabbedRb!= null)
        {
            IGrabbable grabbable = GrabbedRb?.GetComponent<IGrabbable>();
            if (grabbable != null) grabbable.LetGo(this);
        }
        
        GrabbedRb = null;
        hairGenerator.endpoint.GetComponent<Follower>().Target = defaultHairPosition;
        hairGenerator.endpoint.GetComponent<Follower>().SmoothTime = .08f;
        hairGenerator.noiseAmplitude = .25f;
        hairGenerator.endSize = .2f;
        if (springJoint)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint);
            springJoint = null;
        }
        hairGenerator.maxDistance = 1;
        Grabbed = false;
    }

    void Start()
    {
        hairGenerator = GetComponentInChildren<HairGenerator>();
        rigidBody = GetComponent<Rigidbody>();
        hairGenerator.endpoint.parent = null;
        hairGenerator.middlepoint.parent = null;
        LetGo();
    }

    void Update()
    {
        if (!Grabbed)
            hairGenerator.noiseSpeed = .5f + rigidBody.linearVelocity.magnitude / 2;
        else
            hairGenerator.noiseSpeed = .2f;

        if (GrabbedRb != null && !Grabbed && springJoint == null)
            Grab(GrabbedRb);
        else if ((GrabbedRb == null && Grabbed) || (GrabbedRb != null && !Grabbed))
            LetGo();
    }

}
