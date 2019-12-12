using Orbitality.Physics;
using UnityEngine;

namespace Orbitality.SpaceObjects
{
    public class Projectile : SpaceObject, iGravityAffectable
    {
        public System.Action<Vector3> OnExplosion;
        public System.Action<iGravityAffectable> OnProjectileRemoved;
        public System.Action<BattleObject, int, int> OnCollideWithBattleObject;

        public SpriteRenderer ProjectileSpriteRenderer;
        public SpriteRenderer EnemyHighlightSpriteRenderer;
        public TrailRenderer TrailRenderer;

        private Vector2 m_MoveDir;
        private Vector2 m_GravityVector;
        private float m_Acceleration;
        private float m_GravityForce;
        private float m_AutodestroyTime;
        private int m_Damage;
        private int m_SenderID;

        private bool m_IsActive = false;

        public bool IsPlayerSender { get; private set; }
        public override float WorldSize => ProjectileSpriteRenderer != null ? ProjectileSpriteRenderer.transform.localScale.x : 1;


        public void Launch(Vector2 moveDir, float acceleration, float weight, float lifeTime, int damage, Sprite sprite, bool isPlayerSender, int senderID)
        {
            Weight = weight;

            m_MoveDir = moveDir;
            m_Acceleration = acceleration;
            m_AutodestroyTime = Time.time + lifeTime;
            IsPlayerSender = isPlayerSender;
            m_Damage = damage;
            m_SenderID = senderID;

            ProjectileSpriteRenderer.sprite = sprite;
            EnemyHighlightSpriteRenderer.enabled = !isPlayerSender;

            transform.rotation = GetRotationToDirection(moveDir);

            if (!isPlayerSender)
            {
                TrailRenderer.startColor = Color.red;
                TrailRenderer.endColor = TrailRenderer.endColor;
            }

            m_IsActive = true;
        }

        public void SetGravity(Vector3 gravityVector, float gravityForce)
        {
            m_GravityVector = gravityVector;
            m_GravityForce = gravityForce;
        }


        void Update()
        {
            if (m_IsPaused || !m_IsActive)
                return;

            m_MoveDir = transform.right;

            if (Time.time >= m_AutodestroyTime)
            {
                RemoveProjectile(transform.position);
            }
            else
            {
                Vector3 moveDir = m_MoveDir * m_Acceleration + m_GravityVector * m_GravityForce;

                transform.position += moveDir * Time.deltaTime;
                transform.rotation = GetRotationToDirection(moveDir);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            iAttractor attractor = collider.gameObject.GetComponent<iAttractor>();
            if (attractor != null)
            {
                //Please, accept my appologize. I dont like this peace of code but it exists due to the lack of time :)

                //Ignore collision with self 
                BattleObject battleObject = attractor as BattleObject;
                if (battleObject != null && battleObject.ID == m_SenderID)
                    return;

                RemoveProjectile(collider.transform.position);

                if (battleObject != null)
                    OnCollideWithBattleObject?.Invoke(battleObject, m_Damage, m_SenderID);
            }
        }


        void RemoveProjectile(Vector3 explosionPos)
        {
            m_IsActive = false;
            OnProjectileRemoved?.Invoke(this);
            OnExplosion?.Invoke(explosionPos);
        }

        Quaternion GetRotationToDirection(Vector2 dir)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
