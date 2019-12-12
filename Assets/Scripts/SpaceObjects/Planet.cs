using Orbitality.EllipseShapeOrbit;
using Orbitality.Physics;
using Orbitality.Settings;
using UnityEngine;

namespace Orbitality.SpaceObjects
{
    [RequireComponent(typeof(EllipseMotion))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Planet : BattleObject, iAttractor
    {
        public SpriteRenderer PlanetSprite;
        public SpriteRenderer HightlightSprite;

        private float m_OrbitPeriod;
        private EllipseMotion m_Motion;

        public override float WorldSize => PlanetSprite != null ? PlanetSprite.transform.localScale.x : 1;


        public void Init(int hp, Sprite planetSprite, float orbitPeriod, Color hightlightColor, ProjectileStats projectileStats, float planetScaleToWeightMltp, bool isPlayer, int id, float scaleMultiplayer = 0)
        {
            //Data
            m_OrbitPeriod = orbitPeriod;
            
            //Sprites
            PlanetSprite.sprite = planetSprite;
            HightlightSprite.color = hightlightColor;

            //Planet diversity
            PlanetSprite.transform.localScale += PlanetSprite.transform.localScale * scaleMultiplayer;
            Weight = WorldSize * planetScaleToWeightMltp;

            //Adjust collider size
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(WorldSize * 2, WorldSize * 2);

            //Battle object
            InitBattleObject(hp, projectileStats, isPlayer, id);
        }

        public void SetOrbit(EllipseOrbit orbit, float initOrbitPosition)
        {
            //Motion
            m_Motion = GetComponent<EllipseMotion>();
            m_Motion.Init(transform, orbit, m_OrbitPeriod, initOrbitPosition);
        }

        public void StartMotion() => m_Motion?.StartMotion();

        public void StopMotion() => m_Motion?.StopMotion();


        protected override void Update()
        {
            if (m_IsPaused)
                return;

            base.Update();

            m_Motion?.UpdateMotion(Time.deltaTime);
        }
    }
}
