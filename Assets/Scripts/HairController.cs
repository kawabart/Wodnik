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
        springJoint = this.gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        hairGenerator.maxDistance = 2;
        Grabbed = true;
    }
    public void LetGo()
    {
        grabbedRb = null;
        hairGenerator.endpoint.GetComponent<Follower>().Target = defaultHairPosition;
        if (springJoint)
        {
            springJoint.connectedBody = null;
            //springJoint.breakForce = 0;
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
        //springJoint = GetComponent <SpringJoint>();
        hairGenerator.endpoint.parent = null;
        LetGo();
    }


    void Update()
    {
        hairGenerator.noiseSpeed =.2f+ rigidBody.linearVelocity.magnitude/4;
        if (grabbedRb != null && !Grabbed) Grab(grabbedRb);
        if (grabbedRb == null && Grabbed) LetGo();
    }

}
