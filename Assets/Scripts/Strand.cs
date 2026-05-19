using UnityEngine;

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
