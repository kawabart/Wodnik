using UnityEngine;

public class HairGenerator : MonoBehaviour
{
    public Transform startpoint;
    public Transform middlepoint;
    public Transform endpoint;

    public Vector3 middlepointVelocity = Vector3.zero;
    public float middlepointStiffness = .005f;
    public float dampness = .1f;
    void CalculateMidlepoint()
    {
        Vector3 middlepointTargetPosition = Vector3.LerpUnclamped(startpoint.position, endpoint.position, .5f);
        
        middlepointVelocity += (middlepointTargetPosition - middlepoint.transform.position) * middlepointStiffness;
        middlepointVelocity *= dampness;
        middlepoint.transform.position += middlepointVelocity;
       
    }

    void GenerateStrand()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMidlepoint();
    }

    Vector3 Bezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return Mathf.Pow(1 - t, 2) * A +
               2 * (1 - t) * t * B +
               Mathf.Pow(t, 2) * C;
    }
}   
