using UnityEngine;

namespace Orbitality.Effects
{
    public class CameraShakeEffect : MonoBehaviour
    {
        [Tooltip("A measure of magnitude for the shake. Tweak based on your preference")]
        public float ShakeMagnitude = 0.3f;

        [Tooltip("A measure of how quickly the shake effect should evaporate")]
        public float DampingSpeed = 1.0f;

        [Tooltip("A measure of how much time shaking is going to lasts")]
        public float ShakeDuration = 0.2f;

        private float m_CurShakeDuration = 0f;
        private Vector3 m_InitPos;

        void Awake()
        {
            m_InitPos = transform.localPosition;
        }

        void Update()
        {
            if (m_CurShakeDuration > 0)
            {
                transform.localPosition = m_InitPos + Random.insideUnitSphere * ShakeMagnitude;

                m_CurShakeDuration -= Time.deltaTime * DampingSpeed;
            }
            else
            {
                m_CurShakeDuration = 0f;
                transform.localPosition = m_InitPos;
            }
        }

        public void TriggerShake() => m_CurShakeDuration = ShakeDuration;
    }
}
