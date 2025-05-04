using UnityEngine;

namespace ZeroMod.Survivors.Zero.Components
{
    public class ShowHoldIcon : MonoBehaviour
    {
        public GameObject holdIconPrefab;

        public void ShowIcon(float duration = 1.5f)
        {
            if (holdIconPrefab == null) return;

            GameObject canvas = GameObject.Find("HUD");
            if (canvas == null) return;

            GameObject instance = Instantiate(holdIconPrefab, canvas.transform);
            instance.transform.localPosition = Vector3.zero; // centro da tela

            Destroy(instance, duration);
        }
    }
}