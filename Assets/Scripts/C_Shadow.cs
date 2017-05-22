using UnityEngine;
using System.Collections;

public class C_Shadow : MonoBehaviour
{
    public float m_OffsetY = -0.35f;
    public float m_Size = 1.0f;

    public GameObject m_SpawnedObject;

    void Awake()
    {
        GameObject tempSpawn = Resources.Load("Shadow") as GameObject;
        m_SpawnedObject = Instantiate(tempSpawn, transform.position, Quaternion.identity) as GameObject;
        m_SpawnedObject.transform.localScale = new Vector3(m_Size, m_Size, m_Size);
    }

    void LateUpdate()
    {
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y + m_OffsetY);
        m_SpawnedObject.transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
