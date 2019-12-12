using UnityEngine;

namespace Orbitality.Settings
{
    public class GameSettings : MonoBehaviour
    {
        [Header("Weight")]
        [Tooltip("Controls dependency between planet size and its wight to make weight to be proportional to scale")]
        public float PlanetScaleToWeightMltp = 2f;
        [Tooltip("Sun weight :)")]
        public float SunWeight = 2;

        [Header("   -> Player HP")]
        [Header("HP")]
        [Tooltip("Min player HP")]
        public int MinPlayerHP = 30;
        [Tooltip("Max player HP")]
        public int MaxPlayerHP = 100;
        [Header("   -> Enemy HP")]
        [Tooltip("Min enemy HP")]
        public int MinEnemyHP = 10;
        [Tooltip("Max enemy HP")]
        public int MaxEnemyHP = 30;


        [Header("Score")]
        [Tooltip("Amount of scores players gets for destroying an enemy")]
        public int ScoreForPlanet = 125;

        [Header("AI")]
        [Tooltip("Makes AI to shoot each time")]
        public float TimeBetweenShoots = 0.5f;

        [Header("   -> Orbit Period")]
        [Header("Planet Orbit")]
        [Tooltip("Min seconds that takes planet to rotate around the sun")]
        public float MinPlanetOrbitPeriod = 1;
        [Tooltip("Max seconds that takes planet to rotate around the sun")]
        public float MaxPlanetOrbitPeriod = 5;

        [Header("   -> Orbit Offset")]
        [Tooltip("Min offset in base size of the closest to the sun planet")]
        public float MinInitOffset = 0.2f;
        [Tooltip("Max offset in base size of the closest to the sun planet")]
        public float MaxInitOffset = 0.5f;
        [Tooltip("Max offset between planet orbits. Min value is 0")]
        public float MaxPlanetOrbitOffset = 0.12f;

        [Header("   -> Orbit X Focal Change")]
        [Tooltip("Min percent of base orbit offset along X axis (to give round orbit more ellipse shape)")]
        public float MinPlanetOrbitXFocalChange = -0.15f;
        [Tooltip("Max percent of base orbit offset along X axis (to give round orbit more ellipse shape)")]
        public float MaxPlanetOrbitXFocalChange = 0.15f;

        [Header("   -> Orbit Y Focal Change")]
        [Tooltip("Min percent of base orbit offset along Y axis (to give round orbit more ellipse shape)")]
        public float MinPlanetOrbitYFocalChange = -0.1f;
        [Tooltip("Max percent of base orbit offset along Y axis (to give round orbit more ellipse shape)")]
        public float MaxPlanetOrbitYFocalChange = 0.1f;

        [Header("Enemies")]
        [Tooltip("Min amount of enemies")]
        public int MinEnemiesAmount = 1;
        [Tooltip("Max amount of enemies")]
        public int MaxEnemiesAmount = 5;

        [Header("   -> Scale Multiplayer")]
        [Tooltip("Min scale multiplayer for enemy (percent on how initial size should be changed)")]
        public float MinEnemyScaleMltp = -0.2f;
        [Tooltip("Max scale multiplayer for enemy (percent on how initial size should be changed)")]
        public float MaxEnemyScaleMltp = 1.5f;

        [Header("Projectiles")]
        [Tooltip("Variety of projetile stats")]
        public ProjectileStats[] ProjectileStats;
    }

    [System.Serializable]
    public class ProjectileStats
    {
        public float Acceleration = 0;
        public float Weight = 1;
        public float Cooldown = 0.1f;
        public float LifeTime = 2;
        public int Damage = 5;
        public Sprite CorrespondingSprite;
    }
}
