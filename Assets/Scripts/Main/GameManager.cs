using Orbitality.Effects;
using Orbitality.EllipseShapeOrbit;
using Orbitality.Physics;
using Orbitality.Settings;
using Orbitality.SpaceObjects;
using Orbitality.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality.Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public AssetsLibrary AssetsLibrary;
        public GameSettings GameSettings;
        public UIManager UIManager;
        public CameraShakeEffect CameraShakeEffect;
        public Sun SunObject;

        private event System.Action<int> m_OnScoreUpdate;
        private event System.Action<bool> m_OnPauseStateChanged;
        private event System.Action<bool> m_OnGameFinished;

        private int m_TotalEnemies = 0;
        private int m_CurrentEnemies = 0;
        private int m_EnemiesDestroyed = 0;
        private bool m_IsPaused = false;
        private BattleObject m_Player;
        private AIManager m_AIManager;
        private InputManager m_InputManager;

        public PhysicsController PhysicsController { get; private set; }


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            //Subscribe for local events
            m_OnPauseStateChanged += UIManager.HandlePauseChangeState;
            m_OnScoreUpdate += UIManager.UpdateScore;

            //Initialize AI
            m_AIManager = new AIManager(GameSettings.TimeBetweenShoots);

            //Initialize UI
            UIManager.OnPauseChange += ChangePause;
            UIManager.Init();

            //Initialize Input
            m_InputManager = new InputManager();

            //Set weight to the sun
            SunObject.Weight = GameSettings.SunWeight;

            //Initialize physics and add sun as attractor
            PhysicsController = new PhysicsController();
            PhysicsController.AddAttractor(SunObject);

            //Create player and enemies
            CreatePlayers();
        }

        void Update()
        {
            if (!m_IsPaused)
            {
                PhysicsController.UpdatePhysics();
                m_AIManager.UpdateAI(Time.deltaTime);
                m_InputManager.UpdateInput();
            }
        }


        void CreatePlayers()
        {
            List<Planet> planetsInSystem = new List<Planet>();
            int idCounter = 0;

            //Create player and subscribe for events
            Planet planet = AssetsLibrary.Library_Prefabs.CreatePlanetPrefab();
            planet.OnCreateProjectile += CreateProjectile;
            m_OnPauseStateChanged += planet.PauseUpdate;
            m_OnGameFinished += planet.PauseUpdate;
            m_InputManager.OnPlayerShoot += planet.Shoot;
            m_Player = planet;

            planet.Init(GetPlayerRandomHP(),
                        AssetsLibrary.Library_Sprites.GetRandomPlayerSprite(), 
                        GetRandomOrbitPeriod(), 
                        AssetsLibrary.Library_Colors.PlayerHightlightColor,
                        GameSettings.ProjectileStats[Random.Range(0, GameSettings.ProjectileStats.Length)],
                        GameSettings.PlanetScaleToWeightMltp, 
                        true,
                        idCounter++);

            //Add planet to list of planets for further orbit processing
            planetsInSystem.Add(planet);

            //Create enemies
            m_TotalEnemies = Random.Range(GameSettings.MinEnemiesAmount, GameSettings.MaxEnemiesAmount + 1);
            m_CurrentEnemies = m_TotalEnemies;
            for (int i = 0; i < m_TotalEnemies; i++)
            {
                planet = AssetsLibrary.Library_Prefabs.CreatePlanetPrefab();
                planet.OnCreateProjectile += CreateProjectile;

                //Subscribe for pause event
                m_OnPauseStateChanged += planet.PauseUpdate;
                m_OnGameFinished += planet.PauseUpdate;

                planet.Init(GetEnemyRandomHP(),
                            AssetsLibrary.Library_Sprites.GetRandomEnemySprite(), 
                            GetRandomOrbitPeriod(), 
                            AssetsLibrary.Library_Colors.EnemyHightlightColor,
                            GameSettings.ProjectileStats[Random.Range(0, GameSettings.ProjectileStats.Length)],
                            GameSettings.PlanetScaleToWeightMltp,
                            false,
                            idCounter++,
                            GetRandomScale());

                //Add planet to list of planets for further orbit processing
                planetsInSystem.Add(planet);

                //Add planet to physics processing
                PhysicsController.AddAttractor(planet);

                //Add planet to AI processing
                m_AIManager.AddObjectToProcessing(planet);
            }

            ApplyOrbits(planetsInSystem);
        }

        void ApplyOrbits(List<Planet> planetsInSystem)
        {
            Planet prevPlanet = null;
            float prevBaseOrbitSize = SunObject.transform.localScale.x;
            while (planetsInSystem.Count > 0)
            {
                Planet planet = planetsInSystem[Random.Range(0, planetsInSystem.Count)];
                planetsInSystem.Remove(planet);

                float offset = prevPlanet != null ? prevPlanet.WorldSize + planet.WorldSize : Random.Range(GameSettings.MinInitOffset, GameSettings.MaxInitOffset);
                float baseOrbitOffset = prevBaseOrbitSize + offset + Random.Range(0, GameSettings.MaxPlanetOrbitOffset);

                EllipseOrbit orbit = GenerateOrbit(baseOrbitOffset);
                prevBaseOrbitSize = baseOrbitOffset;

                planet.SetOrbit(orbit, Random.Range(0f, 1f));
                planet.StartMotion();

                prevPlanet = planet;
            }
        }

        void CreateProjectile(iAttackableObject sender, float objectSize)
        {
            //Calculate projectile init move direction and spawn offset
            Vector2 dirToSun = SunObject.Position - sender.Position;
            Vector2 verticalToDirToSun = new Vector2(-dirToSun.y, dirToSun.x);
            Vector2 verticalToDirToSunNormalized = verticalToDirToSun.normalized;
            Vector3 spawnOffset2D = verticalToDirToSunNormalized * objectSize;

            //Create and position projectile
            Projectile projectile = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.ProjectilePrefab);
            projectile.transform.position = sender.Position + spawnOffset2D;

            //Subscribe projectile for events
            projectile.OnProjectileRemoved += (iGravityAffectable objToRemove) =>
            {
                //Remove object from physics processing
                PhysicsController.RemoveGravityAffectable(objToRemove);

                //Remove object
                Destroy(objToRemove.GetObject);
            };
            projectile.OnCollideWithBattleObject += (BattleObject objToRemove, int damage, int projectileSenderID) =>
            {
                objToRemove.TakeDamage(damage);

                if (objToRemove.IsDestroyed)
                {
                    //If enemy was destroyed
                    if (!objToRemove.IsPlayer)
                    {
                        //Remove planet from AI processing
                        m_AIManager.RemoveObjectFromProcessing(objToRemove);

                        //If player destroyed enemy
                        if (projectileSenderID == m_Player.ID)
                        {
                            //Update score and calculate win state
                            m_EnemiesDestroyed++;
                            m_OnScoreUpdate?.Invoke(GetScore());
                        }

                        //Decrease amount of enemies
                        m_CurrentEnemies--;

                        //Calculate win state
                        if (PlayerWon())
                        {
                            m_IsPaused = true;
                            UIManager.ShowBattleResult(true, GetScore());
                            m_OnGameFinished?.Invoke(true);
                        }
                        else
                            CameraShakeEffect.TriggerShake();
                    }
                    else //Player was destroyed
                    {
                        m_IsPaused = true;
                        UIManager.ShowBattleResult(false, GetScore());
                        m_OnGameFinished?.Invoke(true);
                    }

                    //Please, accept my appologize. I dont like this peace of code but it exists due to the lack of time :)

                    //Remove planet from physics processing
                    iAttractor attractor = objToRemove as iAttractor;
                    if (attractor != null)
                        PhysicsController.RemoveAttractor(attractor);

                    //Destroy planet
                    Destroy(objToRemove.gameObject);
                }
            };
            projectile.OnExplosion += (Vector3 spawnPos) =>
            {
                Transform explosionEffect = Instantiate(GameManager.Instance.AssetsLibrary.Library_Prefabs.ExplosionPrefab, spawnPos, Quaternion.identity);
                Destroy(explosionEffect.gameObject, 3);
            };

            //Subscribe for pause event
            m_OnPauseStateChanged += projectile.PauseUpdate;
            m_OnGameFinished += projectile.PauseUpdate;

            //Add projectile to physics processing
            PhysicsController.AddGravityAffectable(projectile);

            //Launch projectile
            projectile.Launch(verticalToDirToSunNormalized, 
                              sender.ProjectileStats.Acceleration, 
                              sender.ProjectileStats.Weight, 
                              sender.ProjectileStats.LifeTime,
                              sender.ProjectileStats.Damage,
                              sender.ProjectileStats.CorrespondingSprite,
                              sender.IsPlayer,
                              sender.ID);
        }

        void ChangePause()
        {
            m_IsPaused = !m_IsPaused;
            m_OnPauseStateChanged?.Invoke(m_IsPaused);
        }



        bool PlayerWon() => m_CurrentEnemies <= 0;

        float GetRandomOrbitPeriod() => Random.Range(GameSettings.MinPlanetOrbitPeriod, GameSettings.MaxPlanetOrbitPeriod);

        float GetRandomScale() => Random.Range(GameSettings.MinEnemyScaleMltp, GameSettings.MaxEnemyScaleMltp);

        int GetScore() => m_EnemiesDestroyed * GameSettings.ScoreForPlanet;

        int GetPlayerRandomHP() => Random.Range(GameSettings.MinPlayerHP, GameSettings.MaxPlayerHP + 1);

        int GetEnemyRandomHP() => Random.Range(GameSettings.MinEnemyHP, GameSettings.MaxEnemyHP + 1);

        EllipseOrbit GenerateOrbit(float baseOrbitSize)
        {
            float xAxisFocalChange = Random.Range(GameSettings.MinPlanetOrbitXFocalChange, GameSettings.MaxPlanetOrbitXFocalChange);
            float yAxisFocalChange = Random.Range(GameSettings.MinPlanetOrbitYFocalChange, GameSettings.MaxPlanetOrbitYFocalChange);

            float orbitX = baseOrbitSize + baseOrbitSize * xAxisFocalChange;
            float orbitY = baseOrbitSize + baseOrbitSize * yAxisFocalChange;

            return new EllipseOrbit(orbitX, orbitY);
        }
    }
}
