using UnityEngine;
using System.Collections;

public class C_Broccoli : MonoBehaviour
{
    Transform m_OffsetGrp;

    public float m_ChaseSpeed = 1.0f;
    private Vector2 m_Direction = Vector2.zero;

    private bool m_IsKnockback = false;
    private Vector2 m_KnowbackDirection = Vector2.zero;
    private float m_KnowbackPower = 0.0f;
    private float m_KnowbackTime = 0.0f;
    private float m_KnowbackTimer = 0.0f;

    private GameObject m_Player;

    private Animator m_Animator;
    private bool m_IsHurt = false;
    private float m_HurtTime = 0.16f;
    private float m_HurtTimer = 0.0f;
	private bool dead = false;

    //Shooooooooot ~~~~~~!
    private bool m_IsAttack = false;
    public float m_AttackCoolDownTime = 8.0f;
    private float m_AttackCoolDownTimer = 0.0f;
    private float m_AttackTime = 0.66f;
    private float m_AttackTimer = 0.0f;
    private bool m_IsShoot = false;
    private bool m_IsShootDone = false;
    private float m_ShootTime = 0.5f;
    private float m_ShootTimer = 0.0f;
    public float m_BulletSpeed = 3.0f;

    private GameObject m_Bullet;

    void Awake()
    {
        m_OffsetGrp = transform.Find("Offset_Grp");
        m_Player = GameObject.Find("PlayerControl");
        m_Animator = GetComponentInChildren<Animator>();
        if(m_Player == null)
        {
            Debug.Log("Player is null object");
        }

        m_Bullet = Resources.Load("Broccali_Bullet") as GameObject;
    }
    void Update()
    {
        if(m_IsKnockback == false && !dead)
        {
            ChaseMovement();
            m_AttackCoolDownTimer += Time.deltaTime / m_AttackCoolDownTime;
            if(m_AttackCoolDownTimer >= 1.0f)
            {
                m_AttackCoolDownTimer = 0.0f;
                m_AttackTimer = 0.0f;
                m_ShootTimer = 0.0f;
                m_IsAttack = true;
                m_IsShoot = false;
                m_IsShootDone = false;
            }
        }
        if(m_IsKnockback)
        {
            m_IsAttack = false;
            ExecuteKnockback();
        }

        if(m_IsAttack)
        {
            m_ShootTimer += Time.deltaTime / m_ShootTime;
            if(m_ShootTimer >= 1.0f)
            {
                m_IsShoot = true;
            }
        }

        if(m_IsShoot && m_IsShootDone == false)
        {
            ExecuteShoot();
            m_IsShootDone = true;
        }

        ExecuteAnimation();
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

        if(enemyDirection.x > 0)
        {
            m_OffsetGrp.localScale = new Vector3(1, 1, 1);
        }
        else if(enemyDirection.x < 0)
        {
            m_OffsetGrp.localScale = new Vector3(-1, 1, 1);
        }

        m_Direction = enemyDirection;

        transform.position += new Vector3(tempXSpeed, tempYSpeed, 0);
    }

    private void ExecuteKnockback()
    {
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
        m_IsHurt = true;
        m_IsAttack = false;
        m_HurtTimer = 0.0f;

        if(direction.x < 0)
        {
            m_OffsetGrp.localScale = new Vector3(1, 1, 1);
        }
        else if(direction.x > 0)
        {
            m_OffsetGrp.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ExecuteShoot()
    {
        SpawnBullet(transform.position, m_Direction);
    }

    private void ExecuteAnimation()
    {
        if(m_IsHurt)
        {
            m_HurtTimer += Time.deltaTime / m_HurtTime;
            if(m_HurtTimer >= 1.0f)
            {
                m_IsHurt = false;
                m_Animator.SetBool("IsHurt", false);
            }
            else
            {
                m_Animator.SetBool("IsHurt", true);
            }
        }
        if(m_IsAttack)
        {
            m_AttackTimer += Time.deltaTime / m_AttackTime;
            if(m_AttackTimer >= 1.0f)
            {
                m_IsAttack = false;
                m_Animator.SetBool("IsAttack", false);
            }
            else
            {
                m_Animator.SetBool("IsAttack", true);
            }
        }
        else
        {
            m_Animator.SetBool("IsAttack", false);
        }
    }

    private void SpawnBullet(Vector3 seftPosition, Vector2 direction)
    {
        Vector2 spawnPosition = new Vector2(seftPosition.x, seftPosition.y);
        spawnPosition += direction * 0.1f;
        GameObject tempObject = Instantiate(m_Bullet, spawnPosition, Quaternion.identity) as GameObject;
        tempObject.GetComponent<C_BroccoliBullet>().m_Direction = direction;
        tempObject.GetComponent<C_BroccoliBullet>().m_Speed = m_BulletSpeed;
    }

	public void DeathSentence(bool active){

		dead = true;

		foreach (Collider2D c in GetComponents<Collider2D>()) {
			c.enabled = !active;
		}

		Destroy (GetComponent<Rigidbody2D>());

		m_Animator.SetBool("IsHurt", true);
	}
}
