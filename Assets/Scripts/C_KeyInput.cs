using UnityEngine;
using System.Collections;

public class C_KeyInput : MonoBehaviour
{
    private float m_StoreTime = 0.05f;
    [HideInInspector]
    public int m_NumOfMoveKeyPress = 0;
    [HideInInspector]
    public bool m_PressMovementKey = false;

    [HideInInspector]
    public bool m_IsLeftKey = false;
    private float m_IsLeftKeyTimer;
    public bool m_IsLeftKeyDown = false;

    [HideInInspector]
    public bool m_IsRightKey = false;
    private float m_IsRightKeyTimer;
    public bool m_IsRightKeyDown = false;

    [HideInInspector]
    public bool m_IsDownKey = false;
    private float m_IsDownKeyTimer;
    [HideInInspector]
    public bool m_IsDownKeyDown = false;

    [HideInInspector]
    public bool m_IsUpKey = false;
    private float m_IsUpKeyTimer;
    [HideInInspector]
    public bool m_IsUpKeyDown = false;

    [HideInInspector]
    public bool m_IsMKeyDown = false;

    [HideInInspector]
    public bool m_IsSpaceKey = false;
    private float m_IsSpaceKeyTimer;

    [HideInInspector]
    public bool m_IsEscKey = false;
    private float m_IsEscKeyTimer;

    /*
    void Awake()
    {
    }
    */

    void Update()
    {
        m_NumOfMoveKeyPress = 0;
        PlayerInput();
        if(m_IsLeftKey)
        {
            ++m_NumOfMoveKeyPress;
            m_IsLeftKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsLeftKeyTimer >= 1.0f)
            {
                m_IsLeftKeyTimer = 0.0f;
                m_IsLeftKey = false;
            }
        }
        if(m_IsRightKey)
        {
            ++m_NumOfMoveKeyPress;
            m_IsRightKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsRightKeyTimer >= 1.0f)
            {
                m_IsRightKeyTimer = 0.0f;
                m_IsRightKey = false;
            }
        }
        if(m_IsDownKey)
        {
            ++m_NumOfMoveKeyPress;
            m_IsDownKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsDownKeyTimer >= 1.0f)
            {
                m_IsDownKeyTimer = 0.0f;
                m_IsDownKey = false;
            }
        }
        if(m_IsUpKey)
        {
            ++m_NumOfMoveKeyPress;
            m_IsUpKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsUpKeyTimer >= 1.0f)
            {
                m_IsUpKeyTimer = 0.0f;
                m_IsUpKey = false;
            }
        }
        if(m_IsSpaceKey)
        {
            m_IsSpaceKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsSpaceKeyTimer >= 1.0f)
            {
                m_IsSpaceKeyTimer = 0.0f;
                m_IsSpaceKey = false;
            }
        }
        if(m_IsEscKey)
        {
            m_IsEscKeyTimer += Time.deltaTime / m_StoreTime;
            if(m_IsEscKeyTimer >= 1.0f)
            {
                m_IsEscKeyTimer = 0.0f;
                m_IsEscKey = false;
            }
        }

        //Check is still movement
        if(!m_IsLeftKey && !m_IsRightKey && !m_IsDownKey && !m_IsUpKey)
        {
            m_PressMovementKey = false;
        }
    }

    private void PlayerInput()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            m_IsLeftKeyTimer = 0.0f;
            m_IsLeftKey = true;
            m_PressMovementKey = true;
        }
        else
        {
            //m_IsLeftKey = false;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            m_IsRightKeyTimer = 0.0f;
            m_IsRightKey = true;
            m_PressMovementKey = true;
        }
        else
        {
            //m_IsRightKey = false;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            m_IsUpKeyTimer = 0.0f;
            m_IsUpKey = true;
            m_PressMovementKey = true;
        }
        else
        {
            //m_IsUpKey = false;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            m_IsDownKeyTimer = 0.0f;
            m_IsDownKey = true;
            m_PressMovementKey = true;
        }
        else
        {
            //m_IsDownKey = false;
        }

        m_IsUpKeyDown = Input.GetKeyDown(KeyCode.UpArrow);
        m_IsDownKeyDown = Input.GetKeyDown(KeyCode.DownArrow);
        m_IsLeftKeyDown = Input.GetKeyDown(KeyCode.LeftArrow);
        m_IsRightKeyDown = Input.GetKeyDown(KeyCode.RightArrow);
        m_IsMKeyDown = Input.GetKeyDown(KeyCode.M);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_IsSpaceKeyTimer = 0.0f;
            m_IsSpaceKey = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_IsEscKeyTimer = 0.0f;
            m_IsEscKey = true;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Debug.LogError("Stop");
        }
    }
}
