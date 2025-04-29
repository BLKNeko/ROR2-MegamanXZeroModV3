using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    public class ElectricTrailFollow : MonoBehaviour
    {
        public Vector3 startTransform;
        public Transform target;
        public float effectFollowDuration;

        private LineRenderer lr;
        public float timer = 0f;

        void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }

        void FixedUpdate()
        {
            if (!target || !lr)
                return;

            //lr.SetPosition(0, startTransform);
            //lr.SetPosition(1, endTransform.position);

            Debug.Log("Follow start: " + startTransform);
            Debug.Log("Follow endtr: " + target.position);

            if (lr && target && startTransform != null && timer < effectFollowDuration)
            {
                lr.SetPosition(0, startTransform);
                lr.SetPosition(1, target.position);
                timer += Time.fixedDeltaTime;
                Debug.Log("timer: " + timer);
            }

        }
    }
}
