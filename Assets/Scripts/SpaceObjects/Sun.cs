using Orbitality.Physics;

namespace Orbitality.SpaceObjects
{
    public class Sun : SpaceObject, iAttractor
    {
        public override float WorldSize => 1f;
    }
}
