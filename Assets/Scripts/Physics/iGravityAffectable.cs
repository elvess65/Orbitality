using UnityEngine;

namespace Orbitality.Physics
{
    public interface iGravityAffectable : iPhysicsObject
    {
        void SetGravity(Vector3 gravityVector, float gravityForce);
    }
}
