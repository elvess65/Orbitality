using Orbitality.SpaceObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality.Main
{
    public class AIManager 
    {
        private float m_TimeBetweenShoot;
        private float m_TimeToNextShoot;

        private List<iAttackableObject> m_ProcessingObjects;

        public AIManager(float timeBetweenShoot)
        {
            m_TimeBetweenShoot = timeBetweenShoot;
            m_TimeToNextShoot = m_TimeBetweenShoot;
            m_ProcessingObjects = new List<iAttackableObject>();
        }


        public void AddObjectToProcessing(iAttackableObject planet) => m_ProcessingObjects.Add(planet);

        public void RemoveObjectFromProcessing(iAttackableObject planet)
        {
            if (m_ProcessingObjects.Contains(planet))
                m_ProcessingObjects.Remove(planet);
        }




        public void UpdateAI(float deltaTime)
        {
            m_TimeToNextShoot -= deltaTime;

            if (m_TimeToNextShoot <= 0)
            {
                //Make random enemy to shoot one time in iteration
                m_ProcessingObjects[Random.Range(0, m_ProcessingObjects.Count)].Shoot();

                m_TimeToNextShoot = m_TimeBetweenShoot;
            }
        }
    }
}
