using RoR2;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ZeroMod.Survivors.Zero;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    internal class RaikousenEffect : MonoBehaviour
    {

        public GameObject lightningEffectPrefab; // seu prefab do LineRenderer
        public float effectFollowDuration; // tempo de vida do raio
        public LineRenderer lineRenderer;
        public Transform playerTransform;
        public Vector3 startP;

        private float timer = 0f;

        public void FireLightningEffect(Vector3 startPos, Transform endPos, float dur)
        {

            //Debug.Log("Raikousen EFFECT");


            lightningEffectPrefab = ZeroAssets.raikousenVFX;

            //Debug.Log(lightningEffectPrefab);

            if (!lightningEffectPrefab) return;

            //GameObject lightning = Instantiate(lightningEffectPrefab, startPos, Quaternion.identity);
            //lineRenderer = lightning.GetComponent<LineRenderer>();

            // Importante: Registrar o efeito para ser limpo corretamente
            EffectData effectData = new EffectData
            {
                origin = startPos,
                scale = 1f


            };
            EffectManager.SpawnEffect(lightningEffectPrefab, effectData, true);


            lineRenderer = lightningEffectPrefab.GetComponent<LineRenderer>();
            playerTransform = endPos;
            startP = startPos;

            //if (lineRenderer)
            //{
            //    lineRenderer.SetPosition(0, startPos);
            //    lineRenderer.SetPosition(1, endPos.position);
            //}

            lightningEffectPrefab.AddComponent<ElectricTrailFollow>();

            ElectricTrailFollow follow = lightningEffectPrefab.GetComponent<ElectricTrailFollow>();

            if (follow)
            {
                follow.startTransform = startPos;
                follow.target = playerTransform;
                follow.effectFollowDuration = dur * 0.8f;
            }

            

            //Destroy(lightning, dur);
        }

        void Update()
        {
            //if (lineRenderer && playerTransform && startP != null && timer < effectFollowDuration)
            //{
            //    lineRenderer.SetPosition(0, startP);
            //    lineRenderer.SetPosition(1, playerTransform.position);
            //    timer += Time.deltaTime;
            //}
        }

    }
}
