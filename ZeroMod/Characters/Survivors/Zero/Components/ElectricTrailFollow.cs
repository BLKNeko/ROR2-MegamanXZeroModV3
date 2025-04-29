
using UnityEngine;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    public class ElectricTrailFollow : MonoBehaviour
    {
        public Vector3 startTransform;
        public Transform target;
        public float effectFollowDuration;

        private LineRenderer lr;
        private float timer = 0f;

        private float noiseAmount = 0.5f;
        private int segments = 5;



        void Awake()
        {
            lr = GetComponent<LineRenderer>();
            timer = 0f;
        }

        public void Initilize(Vector3 start, Transform end, float duration, float noise, int seg)
        {
            startTransform = start;
            target = end;
            effectFollowDuration = duration * 0.9f;
            timer = 0f;
            noiseAmount = noise;
            segments = seg;

            Debug.Log("Follow Initilize +++++");
            Debug.Log("Follow start: " + startTransform);
            Debug.Log("Follow endtr: " + target.position);
            Debug.Log("effectFollowDuration: " + effectFollowDuration);
            Debug.Log("timer: " + timer);

        }

        public void Initilize(Vector3 start, Transform end, float duration)
        {
            startTransform = start;
            target = end;
            effectFollowDuration = duration * 0.9f;
            timer = 0f;

            Debug.Log("Follow Initilize ---");
            Debug.Log("Follow start: " + startTransform);
            Debug.Log("Follow endtr: " + target.position);
            Debug.Log("effectFollowDuration: " + effectFollowDuration);
            Debug.Log("timer: " + timer);

        }

        void FixedUpdate()
        {
            if (!target || !lr)
                return;

            //lr.SetPosition(0, startTransform);
            //lr.SetPosition(1, endTransform.position);

            
            //Debug.Log("Follow start: " + startTransform);
            //Debug.Log("Follow endtr: " + target.position);
            //Debug.Log("effectFollowDuration: " + effectFollowDuration);
            //Debug.Log("timer: " + timer);

            if (lr && target && startTransform != null && timer < effectFollowDuration)
            {
                //lr.SetPosition(0, startTransform);
                //lr.SetPosition(1, target.position);

                lr.positionCount = segments;

                for (int i = 0; i < segments; i++)
                {
                    float t = (float)i / (segments - 1);
                    Vector3 point = Vector3.Lerp(startTransform, target.position, t);

                    // Adiciona um pouco de "vibração"
                    point += (Vector3)(Random.insideUnitCircle * noiseAmount);

                    lr.SetPosition(i, point);
                }
                //timer += Time.fixedDeltaTime;
                //Debug.Log("timer: " + timer);
            }

            

        }
    }
}
