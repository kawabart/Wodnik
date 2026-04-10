using UnityEngine;
using System;

public class HairGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Hair grows from here. Should be at player's head.")]
    public Transform startpoint;
    [SerializeField, Tooltip("Middle point with velocity applied. It's position is calculated based of startPoint and endPoint positions (along with stifness, dampness etc).")]
    public Transform middlepoint;
    [SerializeField, Tooltip("Hair ends here. Can be attached to objects.")]
    public Transform endpoint;

    [SerializeField, Tooltip("Default object that's instantiated as hairstrand.")]
    private LineRenderer strandPrefab;
    [Tooltip("Current velocity of the middlepoint.")]
    private Vector3 middlepointVelocity = Vector3.zero;

    public float middlepointStiffness = .1f;
    public float dampness = .9f;
    [SerializeField, Tooltip("Scale applied to offset of the hair roots.")]
    public float startSize = 1f;
    [SerializeField, Tooltip("Scale applied to offset in the middle part of hair. Smaller when hair is streched.")]
    public float middleSize = .5f;
    [SerializeField, Tooltip("Size of the offset at the end of the hair. Could be bigger or smaller based of of the grabbed object size.")]
    public float endSize = 0;
    [Tooltip("Where along the length of hair should its center of mass be. From 0 to 1.")]
    public float middlePointLerp = .5f;
    [Tooltip("Decides the frequency of hair waves.")]
    public float noiseScale = 2f;
    [Tooltip("Hair animation speed. Changes based on player speed.")]
    public float noiseSpeed = 1f;
    [Tooltip("Decides how high the waves of hair are.")]
    public float noiseAmplitude = 0.05f;
    [Tooltip("Distance at which hair are considered fully stretched.")]
    public float maxDistance = 4;

    [SerializeField, Tooltip("Regenerates hair in runtime.")]
    private bool regenerateStrands = false;
    [SerializeField, Tooltip("Nr of vertices on hair strands. Need to regenerate at runtime.")]
    private int resolution = 20;
    [SerializeField, Tooltip("How many strands of hair do we generate. Need to regenerate at runtime.")]
    private int strandCount = 10;
    [SerializeField, Tooltip("Size of max offset of hair. Need to regenerate at runtime.")]
    private float headSize = .5f;
    private Strand[] strands;

    void GenerateStrands()
    {
        strands = new Strand[strandCount];

        for (int i = 0; i < strandCount; i++)
        {
            strands[i] = new Strand(headSize, resolution);
        }
    }
    void RemoveStrands()
    {
        foreach (Strand strand in strands)
        {
            if (strand.strand != null)
                Destroy(strand.strand.gameObject);
        }
    }
    void CalculateMidlepoint()
    {
        Vector3 middlepointTargetPosition = Vector3.LerpUnclamped(startpoint.position, endpoint.position, middlePointLerp);
        float dt = Time.deltaTime;
        middlepointVelocity += (middlepointTargetPosition - middlepoint.position) * middlepointStiffness * dt * 60f;
        middlepointVelocity *= Mathf.Pow(dampness, dt * 60f);
        middlepoint.position += middlepointVelocity * dt * 60f;
    }

    void CalculateStrand(Strand strand)
    {
        float maxMiddleSize = (startSize + endSize) * 1.5f;
        float minMiddleSize = .05f - (startSize + endSize) / 3;

        float distance = (endpoint.position - startpoint.position).magnitude;
        float distanceNormalized = distance / maxDistance;

        middleSize = Mathf.Lerp(maxMiddleSize, minMiddleSize, distanceNormalized);
        if (strand.strand == null)
        {
            strand.strand = GameObject.Instantiate(strandPrefab).GetComponent<LineRenderer>();
            strand.strand.positionCount = resolution;
            strand.strand.colorGradient = strand.gradient;
        }

        Vector3 A = startpoint.position + startpoint.right * strand.offset.x * startSize + startpoint.up * strand.offset.y * startSize;
        Vector3 B = middlepoint.position + startpoint.right * strand.offset.x * middleSize + startpoint.up * strand.offset.y * middleSize;
        Vector3 C = endpoint.position + startpoint.right * strand.offset.x * endSize + startpoint.up * strand.offset.y * endSize;


        Vector3 dir = (C - A).normalized;
        Vector3 side = Vector3.Cross(dir, Vector3.up);

        float time = Time.time;
        float noiseTime = time * noiseSpeed;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 pos = Bezier(A, B, C, t);


            float noise = Mathf.PerlinNoise(
                t * noiseScale + strand.noiseOffset,
                noiseTime
            ) - 0.5f;

            float attenuation = t;
            pos += side * noise * noiseAmplitude * attenuation;

            strand.positions[i] = pos;
        }

        strand.strand.SetPositions(strand.positions);
    }
    void Start()
    {
        GenerateStrands();
    }

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
    public float noiseOffset;
    public Vector3[] positions;
    public Strand(float headSize, int resolution)
    {
        positions = new Vector3[resolution];
        noiseOffset = UnityEngine.Random.Range(0f, 10f);
        float blackness = UnityEngine.Random.Range(.1f, 0.4f);
        color = new Color(blackness, blackness, blackness);
        offset = new Vector3(UnityEngine.Random.Range(-headSize, headSize), UnityEngine.Random.Range(-headSize, headSize), 0);
        gradient = new Gradient();
        gradient.SetColorKeys(new GradientColorKey[] {
            new GradientColorKey(color*.5f, 0f),
            new GradientColorKey(color, 1f)
        });
    }
}
