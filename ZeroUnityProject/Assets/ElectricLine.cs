using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float noiseAmount = 0.2f;
    public float noiseSpeed = 5f;

    private Vector3[] basePositions;

    void Start()
    {
        basePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(basePositions);
    }

    void Update()
    {
        Vector3[] positions = new Vector3[basePositions.Length];
        for (int i = 0; i < basePositions.Length; i++)
        {
            Vector3 noise = new Vector3(
                Mathf.PerlinNoise(Time.time * noiseSpeed + i, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, Time.time * noiseSpeed + i) - 0.5f,
                0f
            ) * noiseAmount;

            positions[i] = basePositions[i] + noise;
        }
        lineRenderer.SetPositions(positions);
    }
}
