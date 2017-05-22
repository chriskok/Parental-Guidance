using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_Wall : MonoBehaviour
{
    private Camera m_Camera;
    private Vector2 m_AspectRation = new Vector2(4, 3);
    private Vector2 m_OffsetPosition = Vector2.zero;
    private float m_Width = 10;
    private float m_Height = 10;

    public float m_OffsetBottomOnly = 0.15f;

    public float m_Tickness = 1.0f;
    public float m_Boarder = 0.2f;

    public List<BoxCollider2D> m_WallList = new List<BoxCollider2D>();
    /*
    0 = top
    1 = botton
    2 = left
    3 = right
    */

    void Awake()
    {
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector2 cameraPos = m_Camera.transform.position;
        m_Width = m_Camera.orthographicSize * 2 * m_AspectRation.x / m_AspectRation.y;
        m_Height = m_Camera.orthographicSize * 2 * (1 - m_OffsetBottomOnly);

        m_OffsetPosition.x = cameraPos.x - (m_Width / 2);
        m_OffsetPosition.y = cameraPos.y - (m_Height / 2) + (m_Camera.orthographicSize * m_OffsetBottomOnly);
        ;

        //------------------
        // Size
        //------------------
        float tempSizeX = 0;
        float tempSizeY = 0;

        // Top
        tempSizeX = m_Width;
        tempSizeY = m_Tickness;
        m_WallList[0].size = new Vector2(tempSizeX, tempSizeY);

        // Bottom
        tempSizeX = m_Width;
        tempSizeY = m_Tickness;
        m_WallList[1].size = new Vector2(tempSizeX, tempSizeY);

        // Left
        tempSizeX = m_Tickness;
        tempSizeY = m_Height;
        m_WallList[2].size = new Vector2(tempSizeX, tempSizeY);

        // Right
        tempSizeX = m_Tickness;
        tempSizeY = m_Height;
        m_WallList[3].size = new Vector2(tempSizeX, tempSizeY);

        //------------------
        // Offset
        //------------------
        float tempOffsetX = 0;
        float tempOffsetY = 0;
        // Top
        tempOffsetX = m_Width / 2;
        tempOffsetY = m_Height + (m_Tickness / 2) - m_Boarder;
        m_WallList[0].offset = new Vector2 (tempOffsetX, tempOffsetY);

        // Bottom
        tempOffsetX = m_Width / 2;
        tempOffsetY = 0 - (m_Tickness / 2) + m_Boarder;
        m_WallList[1].offset = new Vector2(tempOffsetX, tempOffsetY);

        // Left
        tempOffsetX = 0 - (m_Tickness / 2) + m_Boarder;
        tempOffsetY = m_Height / 2;
        m_WallList[2].offset = new Vector2(tempOffsetX, tempOffsetY);

        // Right
        tempOffsetX = m_Width + (m_Tickness / 2) - m_Boarder;
        tempOffsetY = m_Height / 2;
        m_WallList[3].offset = new Vector2(tempOffsetX, tempOffsetY);

        for(int i = 0; i < m_WallList.Count; ++i)
        {
            m_WallList[i].offset += m_OffsetPosition;
        }
    }
}
