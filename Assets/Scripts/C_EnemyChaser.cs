using UnityEngine;
using System.Collections;

    public class C_EnemyChaser : MonoBehaviour
    {
        public float m_ChaseSpeed = 1.0f;

        private bool m_IsKnockback = false;
        private Vector2 m_KnowbackDirection = Vector2.zero;
        private float m_KnowbackPower = 0.0f;
        private float m_KnowbackTime = 0.0f;
        private float m_KnowbackTimer = 0.0f;

        GameObject m_Player;
        GameObject m_HitSplash;

        void Awake()
        {
			m_Player = GameObject.Find("PlayerControl");
            if(m_Player == null)
            {
                Debug.Log("Player is null object");
            }

            m_HitSplash = Resources.Load("VFX_HitSplash") as GameObject;
        }
        void Update()
        {
            if(m_IsKnockback == false)
            {
                ChaseMovement();
            }
            if(m_IsKnockback)
            {
                ExecuteKnockback();
            }
        }

        private void ChaseMovement()
        {
            Vector2 playerPos = m_Player.transform.position;
            Vector2 enemyPos = transform.position;

            Vector2 tempPos = playerPos - enemyPos;
            float distance = tempPos.magnitude;

            Vector2 enemyDirection = tempPos / distance;

            float tempXSpeed = enemyDirection.x * m_ChaseSpeed * Time.deltaTime;
            float tempYSpeed = enemyDirection.y * m_ChaseSpeed * Time.deltaTime;

            transform.position += new Vector3(tempXSpeed, tempYSpeed, 0);
        }

        private void ExecuteKnockback()
        {
            if(m_KnowbackTimer == 0)
            {
                SpawnVFX();
            }
            m_KnowbackTimer += Time.deltaTime / m_KnowbackTime;
            if(m_KnowbackTimer >= 1.0f)
            {
                m_IsKnockback = false;
            }
            float tempXSpeed = m_KnowbackDirection.x * m_KnowbackPower * Time.deltaTime / m_KnowbackTime * (1 - (m_KnowbackTimer * m_KnowbackTimer));
            float tempYSpeed = m_KnowbackDirection.y * m_KnowbackPower * Time.deltaTime / m_KnowbackTime * (1 - (m_KnowbackTimer * m_KnowbackTimer));

            transform.position += new Vector3(tempXSpeed, tempYSpeed, 0);
        }

        public void GetKnockback(Vector2 direction, float knockbackValue, float knockbackTime)
        {
            m_IsKnockback = true;
            m_KnowbackDirection = direction;
            m_KnowbackPower = knockbackValue;
            m_KnowbackTime = knockbackTime;

            m_KnowbackTimer = 0.0f;
        }

        private void SpawnVFX()
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            spawnPosition += m_KnowbackDirection * (-0.4f);
            Instantiate(m_HitSplash, spawnPosition, Quaternion.identity);
        }
    }
