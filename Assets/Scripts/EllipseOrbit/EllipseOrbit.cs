using UnityEngine;

namespace Orbitality.EllipseShapeOrbit
{
    [System.Serializable]
    public class EllipseOrbit
    {
        public float xAxis;
        public float yAxis;

        public EllipseOrbit(float x, float y)
        {
            xAxis = x;
            yAxis = y;
        }

        public Vector2 GetPositionAtOrbit(float time)
        {
            float angle = Mathf.Deg2Rad * 360 * time;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;

            return new Vector2(x, y);
        }
    }
}
