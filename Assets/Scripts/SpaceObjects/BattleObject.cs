using System;
using Orbitality.Settings;
using Orbitality.UI;
using UnityEngine;

namespace Orbitality.SpaceObjects
{
    public abstract class BattleObject : SpaceObject, iAttackableObject
    {
        public event Action<iAttackableObject, float> OnCreateProjectile;

        public PlanetUIController UIController;

        public ProjectileStats ProjectileStats { get; private set; }
        public bool IsPlayer { get; private set; }
        public int ID { get; private set; }

        private int m_InitHP;
        private int m_CurrentHP;
        private float m_CannotShootLeftTime = 0;

        public bool IsDestroyed => m_CurrentHP <= 0;

        protected bool CanShoot => m_CannotShootLeftTime <= 0;


        public void Shoot()
        {
            if (CanShoot)
            {
                OnCreateProjectile?.Invoke(this, WorldSize);

                m_CannotShootLeftTime = ProjectileStats.Cooldown;

                if (IsPlayer)
                    UIController.ShowReloadBar(true);
            }
        }

        public void TakeDamage(int damage)
        {
            m_CurrentHP -= damage;

            UIController.ShowHPBar(true);
            UIController.HPBarController.UpdateValue(m_CurrentHP);
        }


        protected void InitBattleObject(int hp, ProjectileStats projectileStats, bool isPlayer, int id)
        {
            ID = id;
            IsPlayer = isPlayer;
            m_InitHP = m_CurrentHP = hp;
            ProjectileStats = projectileStats;

            //UI
            UIController.transform.localPosition += new Vector3(0, WorldSize, 0);
            UIController.Init(m_InitHP);
        }

        protected void ProcessReload()
        {
            if (!CanShoot)
            {
                if (IsPlayer)
                    UIController.ReloadBarController.UpdateValue(1 - m_CannotShootLeftTime / ProjectileStats.Cooldown);

                m_CannotShootLeftTime -= Time.deltaTime;

                if (IsPlayer && CanShoot)
                    UIController.ShowReloadBar(false);
            }
        }

        protected virtual void Update()
        {
            if (m_IsPaused)
                return;

            ProcessReload();
        }
    }
}
