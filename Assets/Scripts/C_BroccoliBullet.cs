using UnityEngine;
using System.Collections;

public class C_BroccoliBullet : MonoBehaviour
{
    public Vector2 m_Direction = Vector2.zero;
    public float m_Speed = 0.0f;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Travel();
    }

    void OnTriggerEnter2D(Collider2D entity)
    {
        if(entity.CompareTag("Player"))
        {
            GetHit();
        }
    }

    private void Travel()
    {
        Vector2 tempSpeed = m_Direction * m_Speed * Time.deltaTime;
        transform.position += new Vector3(tempSpeed.x, tempSpeed.y, 0);
    }

    public void GetHit()
    {
        Destroy(gameObject);
    }
}
