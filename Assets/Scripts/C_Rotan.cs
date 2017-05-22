using UnityEngine;
using System.Collections;

public class C_Rotan : MonoBehaviour
{
    Transform m_OffsetGrp;

    public float m_ChaseSpeed = 1.0f;

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

    void Awake()
    {
        m_OffsetGrp = transform.Find("Offset_Grp");
        m_Player = GameObject.Find("PlayerControl");
        m_Animator = GetComponentInChildren<Animator>();
        if(m_Player == null)
        {
            Debug.Log("Player is null object");
        }
    }
    void Update()
    {
        if(m_IsKnockback == false && !dead)
        {
            ChaseMovement();
        }
        if(m_IsKnockback)
        {
            ExecuteKnockback();
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

    public void ExecuteAnimation()
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
