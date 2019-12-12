using Orbitality.Physics;
using Orbitality.Settings;
using UnityEngine;

namespace Orbitality.SpaceObjects
{
    public interface iAttackableObject : iPhysicsObject
    {
        event System.Action<iAttackableObject, float> OnCreateProjectile;

        ProjectileStats ProjectileStats { get; }
        bool IsPlayer { get; }
        int ID { get; }

        void Shoot();
    }
}
