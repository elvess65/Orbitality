using System.Collections.Generic;
using UnityEngine;

namespace Orbitality.Physics
{
    public class PhysicsController
    {
        private const float m_G = 6.67f / 100;

        private List<iAttractor> m_Attractors;
        private List<iGravityAffectable> m_GravityAffectables;


        public PhysicsController()
        {
            m_Attractors = new List<iAttractor>();
            m_GravityAffectables = new List<iGravityAffectable>();
        }


        public void AddAttractor(iAttractor attractor) => m_Attractors.Add(attractor);

        public void RemoveAttractor(iAttractor attractor)
        {
            if (m_Attractors.Contains(attractor))
                m_Attractors.Remove(attractor);
        }


        public void AddGravityAffectable(iGravityAffectable gravityAffectable) => m_GravityAffectables.Add(gravityAffectable);

        public void RemoveGravityAffectable(iGravityAffectable gravityAffectable)
        {
            if (m_GravityAffectables.Contains(gravityAffectable))
                m_GravityAffectables.Remove(gravityAffectable);
        }


        public void UpdatePhysics()
        {
            foreach (iGravityAffectable affectableObj in m_GravityAffectables)
            {
                if (affectableObj == null)
                    continue;

                float gravityForce = 0;
                Vector3 gravityDir = Vector3.zero;

                foreach (iAttractor attractor in m_Attractors)
                {
                    if (attractor == null)
                        continue;

                    Vector3 dirToAttractor = attractor.Position - affectableObj.Position;
                    gravityDir += dirToAttractor.normalized;

                    gravityForce += m_G * (attractor.Weight * affectableObj.Weight) / (Mathf.Pow(dirToAttractor.magnitude, 2));
                }

                affectableObj.SetGravity(gravityDir.normalized, gravityForce);
            }
        }
    }
}
