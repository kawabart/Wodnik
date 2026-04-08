using UnityEngine;
using System;
public class HairGenerator : MonoBehaviour
{
    public Transform startpoint;
    public Transform middlepoint;
    public Transform endpoint;

    private LineRenderer lineRenderer;
    public LineRenderer strandPrefab;
    public Vector3 middlepointVelocity = Vector3.zero;
    public float middlepointStiffness = .005f;
    public float dampness = .1f;
    public int resolution = 20;
    public bool regenerateStrands = false;

    public float startSize = 1f;
    public float middleSize = .5f;
    public float endSize = 0;
    
    public int strandCount = 10;
    public float headSize = .5f;
    public Strand[] strands;
    public float middlePointLerp = .5f;
    void GenerateStrands()
    {
        strands = new Strand[strandCount];
 
    }
    void RemoveStrands()
    {
        foreach (Strand strand in strands)
            Destroy(strand.strand.gameObject);
    }
    void CalculateMidlepoint()
    {
        Vector3 middlepointTargetPosition = Vector3.LerpUnclamped(startpoint.position, endpoint.position, middlePointLerp);
        
        middlepointVelocity += (middlepointTargetPosition - middlepoint.transform.position) * middlepointStiffness;
        middlepointVelocity *= dampness;
        middlepoint.transform.position += middlepointVelocity;
       
    }

    void CalculateStrand(Strand strand)
    {
        
        if (strand.strand==null) strand.strand = GameObject.Instantiate(strandPrefab).GetComponent<LineRenderer>();
        lineRenderer = strand.strand;   
        lineRenderer.positionCount = resolution;
        //lineRenderer.endColor = strand.color;
         lineRenderer.colorGradient = strand.gradient;

        Vector3 A = startpoint.position+startpoint.right*strand.offset.x * startSize + startpoint.up*strand.offset.y * startSize;
        Vector3 B = middlepoint.position + startpoint.right * strand.offset.x * middleSize + startpoint.up * strand.offset.y* middleSize;
        Vector3 C = endpoint.position + startpoint.right * strand.offset.x * endSize + startpoint.up * strand.offset.y * endSize;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 pos = Bezier(A, B, C, t);
            lineRenderer.SetPosition(i, pos);
        }
    }
    void Start()
    {
        GenerateStrands();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenerateStrands)
        {
            RemoveStrands();
            GenerateStrands();
            regenerateStrands = false;

        }
        CalculateMidlepoint();
        foreach (Strand strand in strands)
            CalculateStrand(strand);
    }

    Vector3 Bezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return Mathf.Pow(1 - t, 2) * A +
               2 * (1 - t) * t * B +
               Mathf.Pow(t, 2) * C;
    }
}   
[System.Serializable]
public class Strand
{
    public LineRenderer strand;
    public Vector3 offset;
    public Color color;
    public Gradient gradient;
    public Strand( )
    {
        float headSize = .5f;
        float blackness = UnityEngine.Random.Range(0, 0.1f);
        color = new Color(blackness, blackness, blackness);
      offset = new Vector3(UnityEngine.Random.Range(-headSize, headSize), UnityEngine.Random.Range(-headSize, headSize), 0);
        gradient = new Gradient();
        gradient.SetColorKeys(new GradientColorKey[] {
            new GradientColorKey(color*.1f, 0f),
            new GradientColorKey(color, 1f)
        });
    }
}
