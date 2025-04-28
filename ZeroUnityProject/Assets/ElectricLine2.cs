using UnityEngine;

public class ElectricLine2 : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    public float noiseAmount = 0.5f;
    public int segments = 5;

    void Update()
    {
        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            Vector3 point = Vector3.Lerp(startPoint.position, endPoint.position, t);

            // Adiciona um pouco de "vibração"
            point += (Vector3)(Random.insideUnitCircle * noiseAmount);

            lineRenderer.SetPosition(i, point);
        }
    }
}
