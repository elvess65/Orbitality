using Orbitality.Physics;
using UnityEngine;

namespace Orbitality.SpaceObjects
{
    public abstract class SpaceObject : MonoBehaviour, iPhysicsObject
    {
        public float Weight { get; set; }
        public abstract float WorldSize { get; }
        public Vector3 Position => transform.position;
        public GameObject GetObject => gameObject;

        protected bool m_IsPaused = false;


        public void PauseUpdate(bool isPaused) => m_IsPaused = isPaused;
    }
}
