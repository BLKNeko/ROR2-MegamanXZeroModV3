using UnityEngine;

public class ElectricTrailFollow : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!startTransform || !endTransform || !lr)
            return;

        lr.SetPosition(0, startTransform.position);
        lr.SetPosition(1, endTransform.position);
    }
}
