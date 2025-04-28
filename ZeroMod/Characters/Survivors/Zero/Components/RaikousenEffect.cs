using RoR2;
using UnityEngine;
using UnityEngine.PlayerLoop;
using ZeroMod.Survivors.Zero;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    internal class RaikousenEffect : MonoBehaviour
    {

        public GameObject lightningEffectPrefab; // seu prefab do LineRenderer
        public float duration = 2f; // tempo de vida do raio
        public LineRenderer lineRenderer;
        public Transform playerTransform;
        public Vector3 startP;

        public void FireLightningEffect(Vector3 startPos, Transform endPos)
        {

            Debug.Log("Raikousen EFFECT");
            

            lightningEffectPrefab = ZeroAssets.raikousenVFX;
            //lightningEffectPrefab.AddComponent<ElectricTrailFollow>();

            Debug.Log(lightningEffectPrefab);

            if (!lightningEffectPrefab) return;

            GameObject lightning = Instantiate(lightningEffectPrefab, startPos, Quaternion.identity);
            lightning.AddComponent<ElectricTrailFollow>();
            lineRenderer = lightning.GetComponent<LineRenderer>();
            playerTransform = endPos;
            startP = startPos;

            if (lineRenderer)
            {
                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos.position);
            }

            ElectricTrailFollow trail = lightning.GetComponent<ElectricTrailFollow>();
            if (trail)
            {
                trail.startTransform = startPos; // posição inicial
                trail.endTransform = playerTransform; // posição atual do personagem
            }

            // Importante: Registrar o efeito para ser limpo corretamente
            EffectData effectData = new EffectData
            {
                origin = startPos,
                genericFloat = duration
            };
            EffectManager.SpawnEffect(lightning, effectData, transmit: true);

            Destroy(lightning, duration);
        }

        void Update()
        {
            if(lineRenderer && playerTransform && startP != null)
            {
                lineRenderer.SetPosition(0, startP);
                lineRenderer.SetPosition(1, playerTransform.position);
            }
        }

    }
}
