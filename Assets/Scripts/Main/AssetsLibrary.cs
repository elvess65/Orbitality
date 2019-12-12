using Orbitality.SpaceObjects;
using UnityEngine;

namespace Orbitality.Main
{
    public class AssetsLibrary : MonoBehaviour
    {
        public SpritesLibrary Library_Sprites;
        public PrefabsLibrary Library_Prefabs;
        public ColorsLibrary Library_Colors;


        [System.Serializable]
        public class PrefabsLibrary
        {
            public Planet PlanetPrefab;
            public Projectile ProjectilePrefab;
            public Transform ExplosionPrefab;

            public Planet CreatePlanetPrefab()
            {
                try
                {
                    return Instantiate(PlanetPrefab);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error getting planet prefab: { e.Message}");
                    return null;
                }
            }
        }

        [System.Serializable]
        public class SpritesLibrary
        {
            public Sprite[] PlayerPlanetSprites;
            public Sprite[] EnemyPlanetSprites;

            public Sprite GetRandomPlayerSprite()
            {
                try
                {
                    return PlayerPlanetSprites[Random.Range(0, PlayerPlanetSprites.Length)];
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error getting player sprite: {e.Message}");
                    return null;
                }
            }

            public Sprite GetRandomEnemySprite()
            {
                try
                {
                    return EnemyPlanetSprites[Random.Range(0, EnemyPlanetSprites.Length)];
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error getting enemy sprite: {e.Message}");
                    return null;
                }
            }
        }

        [System.Serializable]
        public class ColorsLibrary
        {
            public Color PlayerHightlightColor;
            public Color EnemyHightlightColor;

            public Color VictoryColor;
            public Color DefeatColor;
        }
    }
}
