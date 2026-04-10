using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField] HairGenerator hairGenerator;
    private Rigidbody rigidBody;
    [SerializeField, Tooltip("Default position that hair end lerps to when it isn't attached to anything.")]
    private Transform defaultHairPosition;
    [SerializeField, Tooltip("Rigidbody of object that's currently grabbed by the hair. When null, hair returns to their default position. Drag and drop rigidbody to attach it.")]
    private Rigidbody grabbedRb;
    public bool Grabbed = false;
    [SerializeField, Tooltip("Spring joint thats dynamically created (and removed) to attach rb thats grabbed by hair.")]
    private SpringJoint springJoint;

    public void Grab(Rigidbody rb)
    {
        grabbedRb = rb;
        hairGenerator.endpoint.GetComponent<Follower>().Target = rb.transform;
        hairGenerator.endpoint.GetComponent<Follower>().SmoothTime = 0f;
        hairGenerator.noiseAmplitude = .1f;
        springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        hairGenerator.maxDistance = 1;
        //to do: endsize can change based on dimensions of grabbed object.
        //hairGenerator.endSize = .2f;
        Grabbed = true;
    }

    public void LetGo()
    {
        grabbedRb = null;
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
        if (!Grabbed) hairGenerator.noiseSpeed = .2f + rigidBody.linearVelocity.magnitude / 2;
        else hairGenerator.noiseSpeed = .2f;
        if (grabbedRb != null && !Grabbed && springJoint == null) Grab(grabbedRb);
        else if ((grabbedRb == null && Grabbed) || (grabbedRb != null && !Grabbed)) LetGo();
    }
}
