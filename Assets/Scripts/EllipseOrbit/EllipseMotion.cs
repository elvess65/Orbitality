using UnityEngine;

namespace Orbitality.EllipseShapeOrbit
{
    public class EllipseMotion : MonoBehaviour
    {
        private Transform m_ControlledTransform;
        private EllipseOrbit m_OrbitData;
        private float m_OrbitPeriod;
        private float m_OrbitProgress = 0;

        private const float m_MIN_ORBIT_PERIOD = 0.1f;

        public bool IsActive { get; private set; }


        public void Init(Transform controlledTransform, EllipseOrbit orbitData, float orbitPeriod, float initOrbitPosition)
        {
            m_ControlledTransform = controlledTransform;
            m_OrbitData = orbitData;
            m_OrbitPeriod = orbitPeriod;
            m_OrbitProgress = initOrbitPosition;

            SetOrbitPosition();
        }

        public void StartMotion()
        {
            SetOrbitPosition();

            IsActive = true;
        }

        public void StopMotion() => IsActive = false;

        public void UpdateMotion(float deltaTime)
        {
            if (IsActive)
            {
                m_OrbitProgress += 1f / Mathf.Clamp(m_OrbitPeriod, m_MIN_ORBIT_PERIOD, m_OrbitPeriod) * deltaTime;
                m_OrbitProgress %= 1;

                SetOrbitPosition();
            }
        }


        void SetOrbitPosition() => m_ControlledTransform.position = m_OrbitData.GetPositionAtOrbit(m_OrbitProgress);
    }
}
