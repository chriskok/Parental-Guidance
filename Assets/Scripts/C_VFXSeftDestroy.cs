using UnityEngine;
using System.Collections;

public class C_VFXSeftDestroy : MonoBehaviour
{
    public float m_StayTime = 0.0f;
    private float m_StayTimer = 0.0f;
    void Awake()
    {

    }
    void Update()
    {
        m_StayTimer += Time.deltaTime / m_StayTime;
        if(m_StayTimer >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
