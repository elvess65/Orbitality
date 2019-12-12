using UnityEngine;

namespace Orbitality.Physics
{
    public interface iPhysicsObject
    {
        float Weight { get; set; }
        float WorldSize { get; }
        Vector3 Position { get; }
        GameObject GetObject { get; }
    }
}
