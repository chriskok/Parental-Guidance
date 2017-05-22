using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_PlayerControl : MonoBehaviour
{
    private C_KeyInput m_KeyInput;
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_OffsetGrp;

    private bool m_IsAttack = false;
    private bool m_IsAttackAnim = false;
    private bool m_IsMoving = false;
    private bool m_IsHurt = false;

    public float m_MoveSpeed = 1.0f;
    private Vector2 m_Direction = Vector2.zero;
    private bool m_IsStopMovement = false;
    private bool m_IsStopAction = false;

    public float m_AttackSpeed = 0.2f;
    private float m_AttackSpeedTimer = 0.0f;
    private bool m_AttackHitDelayDone = false;
    private float m_AttackHitDelayTime = 0.1f;
    private float m_AttackHitDelayTimer = 0.0f;
    private float m_AttackAnimTime = 0.33f;
    private float m_AttackAnimTimer = 0.0f;
    private bool m_IsAttackReset = false;

    //private float m_HurtTime = 0.16f;
    private float m_HurtTime = 5.0f;
    private float m_HurtTimer = 0.0f;
    public bool m_IsDead = false;
    private float m_DissappearTime = 1.5f;
    private float m_DissappearTimer = 0.0f;

    public float m_KnockbackRange = 1.0f;
    public float m_KnockbackRadius = 0.5f;
    public float m_KnockbackPower = 1.0f;
    public float m_KnockbackTime = 1.0f;

    private Animator m_Animator;

    private GameObject m_HitSplash;
    private SpriteRenderer m_spriteRenderer;

	private AudioSource m_AudioSource;
	public AudioClip m_SwingSound;
	public AudioClip m_HitSound;

    void Awake()
    {
        m_KeyInput = GetComponent<C_KeyInput>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        m_OffsetGrp = transform.Find("Offset_Grp");

        m_HitSplash = Resources.Load("VFX_HitSplash") as GameObject;
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		m_AudioSource = GetComponent<AudioSource> ();
    }
    void Update()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        if(m_IsStopMovement == false)
        {
            if(m_KeyInput.m_PressMovementKey == true)
            {
                GetMovement();
                /*
                GameObject collisionObject = null;
                collisionObject = CheckNextDirectionCollider(transform.position, m_Direction);
                if(collisionObject == null)
                {
                    ExecuteMovement();
                }
                */
                ExecuteMovement();

                m_IsMoving = true;
            }
            else
            {
                m_IsMoving = false;
            }
        }
        if(m_IsStopAction == false)
        {
            Action();
        }

        if(m_IsHurt)
        {
            if(m_IsDead == false)
            {
                m_DissappearTimer += Time.deltaTime / m_DissappearTime;
                if(m_DissappearTimer >= 1.0f)
                {
                    m_IsDead = true;
                }
                DissappearAnimation(m_DissappearTimer);
            }
        }

        if(m_IsDead)
        {
            Dissappear();
        }

        if(m_IsDead == false)
        {
            ExecuteAnimation();
        }
    }

    void OnTriggerEnter2D(Collider2D entitry)
    {
		if(entitry.CompareTag("EnemyRotan") || entitry.CompareTag("EnemyBullet") || entitry.CompareTag("EnemyBroco"))
		{
			m_AudioSource.PlayOneShot (m_HitSound);
            m_HurtTimer = 0.0f;
            m_IsHurt = true;
            m_IsStopMovement = true;
            m_IsStopAction = true;
			GM gmScript = GameObject.Find("GameManager").GetComponent<GM>();
			if (entitry.tag == "EnemyRotan") {
				gmScript.GameOver (2);
			} else if (entitry.tag == "EnemyBroco" || entitry.tag == "EnemyBullet") {
				gmScript.GameOver (3);
			}
        }
    }

    private void GetMovement()
    {
        if(m_KeyInput.m_NumOfMoveKeyPress < 3)
        {
            float tempX = 0.0f;
            float tempY = 0.0f;

            if(m_KeyInput.m_IsUpKey)
            {
                tempY += 1f;
                m_KeyInput.m_IsUpKey = false;
            }
            if(m_KeyInput.m_IsDownKey)
            {
                tempY -= 1f;
                m_KeyInput.m_IsDownKey = false;
            }
            if(m_KeyInput.m_IsLeftKey)
            {
                tempX -= 1f;
                m_KeyInput.m_IsLeftKey = false;
            }
            if(m_KeyInput.m_IsRightKey)
            {
                tempX += 1f;
                m_KeyInput.m_IsRightKey = false;
            }

            m_Direction.x = tempX;
            m_Direction.y = tempY;
        }
    }

    private void ExecuteMovement()
    {
        float tempXSpeed = m_Direction.x * m_MoveSpeed * Time.deltaTime;
        float tempYSpeed = m_Direction.y * m_MoveSpeed * Time.deltaTime;
        if(m_KeyInput.m_NumOfMoveKeyPress == 2)
        {
            tempXSpeed *= 0.667f;
            tempYSpeed *= 0.667f;
        }

        if(m_Direction.x > 0)
        {
            m_OffsetGrp.localScale = new Vector3(1, 1, 1);
        }
        else if(m_Direction.x < 0)
        {
            m_OffsetGrp.localScale = new Vector3(-1, 1, 1);
        }
        //m_Rigidbody2D.AddForce(new Vector2(tempXSpeed, tempYSpeed));
        transform.position += new Vector3(tempXSpeed, tempYSpeed, 0);
    }

    private void Action()
    {
        if(m_KeyInput.m_IsMKeyDown)
        {
            if(m_IsAttack == false)
            {
                m_AttackSpeedTimer = 0.0f;
                m_AttackAnimTimer = 0.0f;
                m_AttackHitDelayTimer = 0.0f;
                m_IsAttack = true;
                m_IsAttackAnim = true;
                m_IsAttackReset = true;
                m_AttackHitDelayDone = false;

                m_IsStopMovement = true;

				m_AudioSource.PlayOneShot (m_SwingSound);
                //KnockBackEnemy(m_Direction, m_KnockbackRange, m_KnockbackRadius, m_KnockbackPower, m_KnockbackTime);
            }
        }
        if(m_IsAttack)
        {
            m_AttackHitDelayTimer += Time.deltaTime / m_AttackHitDelayTime;
            if(m_AttackHitDelayTimer >= 1.0f && m_AttackHitDelayDone == false)
            {
                m_AttackHitDelayDone = true;
                KnockBackEnemy(m_Direction, m_KnockbackRange, m_KnockbackRadius, m_KnockbackPower, m_KnockbackTime);
            }

            m_AttackSpeedTimer += Time.deltaTime / m_AttackSpeed;
            if(m_AttackSpeedTimer >= 1.0f)
            {
                m_IsStopMovement = false;
                m_IsAttack = false;
            }
        }
    }

    private void KnockBackEnemy(Vector2 direction, float range, float radius, float knockbackValue, float knockbackTime)
    {
        Vector2 startPosition = new Vector2(transform.position.x + 0f,
                                            transform.position.y + 0f);
        Vector2 castDirection = direction;

        if(m_KeyInput.m_NumOfMoveKeyPress < 2)
        {
            startPosition += castDirection * 0.501f;
        }
        else
        {
            castDirection *= 0.77f;
            startPosition += castDirection * 0.501f;
        }

        Vector2 endPoint = startPosition + (direction * range);
        Debug.DrawLine(startPosition, endPoint, Color.yellow, 1.0f);

        Collider2D[] tempColliderArray = { };

        tempColliderArray = Physics2D.OverlapCircleAll(endPoint, radius, Physics2D.DefaultRaycastLayers);

        //RaycastHit2D outInfo = Physics2D.Raycast(startPosition, castDirection, range, Physics2D.DefaultRaycastLayers);
        GameObject colliderObject = null;
        for(int i = 0; i < tempColliderArray.Length; ++i)
        {
            colliderObject = tempColliderArray[i].gameObject;
            if(colliderObject.GetComponent<C_Rotan>() != null)
            {
				m_AudioSource.PlayOneShot (m_HitSound);
                SpawnVFX(colliderObject.transform.position, castDirection);
                colliderObject.GetComponent<C_Rotan>().GetKnockback(castDirection, knockbackValue, knockbackTime);
            }
            if(colliderObject.GetComponent<C_Broccoli>() != null)
			{
				m_AudioSource.PlayOneShot (m_HitSound);
                SpawnVFX(colliderObject.transform.position, castDirection);
                colliderObject.GetComponent<C_Broccoli>().GetKnockback(castDirection, knockbackValue, knockbackTime);
            }
            if(colliderObject.GetComponent<C_BroccoliBullet>() != null)
			{
				m_AudioSource.PlayOneShot (m_HitSound);
                SpawnVFX(colliderObject.transform.position, castDirection);
                colliderObject.GetComponent<C_BroccoliBullet>().GetHit();
                
            }
        }
    }

    private void ExecuteAnimation()
    {
        if(m_IsMoving)
        {
            m_Animator.SetBool("IsRun", true);
        }
        else
        {
            m_Animator.SetBool("IsRun", false);
        }
        if(m_IsAttackAnim)
        {
            m_AttackAnimTimer += Time.deltaTime / m_AttackAnimTime;
            if(m_AttackAnimTimer >= 1)
            {
                m_IsAttackAnim = false;
                m_Animator.SetBool("IsAttack", false);
            }
            else
            {
                m_Animator.SetBool("IsAttack", true);
            }
        }

        if(m_IsAttackReset)
        {
            m_Animator.SetBool("IsAttack", false);
            m_IsAttackReset = false;
        }

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

    private void SpawnVFX(Vector3 enemyPosition, Vector2 direction)
    {
        Vector2 spawnPosition = new Vector2(enemyPosition.x, enemyPosition.y);
        spawnPosition += direction * (-0.4f);
        Instantiate(m_HitSplash, spawnPosition, Quaternion.identity);
    }

    private void DissappearAnimation(float timer)
    {
        if(timer > 0.3f)
        {
            float tempNum = 1 - ((timer - 0.3f) / 0.7f);
            m_spriteRenderer.color = new Vector4(tempNum, tempNum, tempNum, 1);
        }
    }

    private void Dissappear()
    {
        m_OffsetGrp.gameObject.SetActive(false);
        C_Shadow tempShadow = GetComponent<C_Shadow>();
        tempShadow.m_SpawnedObject.SetActive(false);
    }

    /*
    public GameObject CheckNextDirectionCollider(Vector2 objectPosition, Vector2 moveDirection, int layer = Physics2D.DefaultRaycastLayers)
    {
        Vector2 startPosition = new Vector2(objectPosition.x + 0.5f, objectPosition.y + 0.5f);
        Vector2 castDirection = moveDirection;

        if(m_KeyInput.m_NumOfMoveKeyPress < 2)
        {
            startPosition += moveDirection * 0.501f;
        }
        else
        {
            startPosition += moveDirection * 0.501f * 0.75f;
        }

        RaycastHit2D outInfo = Physics2D.Raycast(startPosition, castDirection, 0.1f, layer);
        GameObject colliderObject = null;
        if(outInfo.collider != null)
        {
            colliderObject = outInfo.collider.gameObject;
        }

        return colliderObject;
    }
    */
}
