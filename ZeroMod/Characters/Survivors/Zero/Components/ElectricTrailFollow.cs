using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    public class ElectricTrailFollow : MonoBehaviour
    {
        public Vector3 startTransform;
        public Transform endTransform;
        private LineRenderer lr;

        void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (!endTransform || !lr)
                return;

            lr.SetPosition(0, startTransform);
            lr.SetPosition(1, endTransform.position);
        }
    }
}
