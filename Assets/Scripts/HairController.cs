using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField] HairGenerator hairGenerator;
    Rigidbody rigidBody;
    [SerializeField] Transform defaultHairPosition;
    public Rigidbody grabbedRb;
    public bool Grabbed = false;
    public SpringJoint springJoint;

    public void Grab(Rigidbody rb)
    {
        grabbedRb = rb;
        hairGenerator.endpoint.GetComponent<Follower>().Target = rb.transform;
        hairGenerator.endpoint.GetComponent<Follower>().SmoothTime = 0f;
        hairGenerator.noiseAmplitude = .1f;
        springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        hairGenerator.maxDistance = 2;
        //to do: endsize can change based on dimensions of grabbed object
        //hairGenerator.endSize = .2f;
        Grabbed = true;
    }

    public void LetGo()
    {
        grabbedRb = null;
        hairGenerator.endpoint.GetComponent<Follower>().Target = defaultHairPosition;
        hairGenerator.endpoint.GetComponent<Follower>().SmoothTime = .05f;
        hairGenerator.noiseAmplitude = .25f;
        hairGenerator.endSize = .2f;
        if (springJoint)
        {
            springJoint.connectedBody = null;
            Destroy(springJoint);
            springJoint = null;
        }
        hairGenerator.maxDistance = .8f;
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
        if (grabbedRb != null && !Grabbed) Grab(grabbedRb);
        if (grabbedRb == null && Grabbed) LetGo();
    }

}
