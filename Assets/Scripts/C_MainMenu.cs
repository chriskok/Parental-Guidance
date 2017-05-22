using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class C_MainMenu : MonoBehaviour
{
    private C_KeyInput m_KeyInput;

    private AudioSource m_AudioSource;
    //public AudioClip m_BGMClip;
    public AudioClip m_ButtonSoundClip;
    public AudioClip m_ButtonMoveSoundClip;

    public List<GameObject> m_Button = new List<GameObject>();
    // 0 = Play Game
    // 1 = Help Map
    // 2 = Credit
    // 3 = Exit
    private int m_ButtonIndex = 0;

    private bool m_IsMainMenu = true;
    private bool m_IsHelp1 = false;
    private bool m_IsHelp2 = false;
    private bool m_IsCredit = false;

    public List<GameObject> m_Scene = new List<GameObject>();

    void Awake()
    {
        m_KeyInput = GetComponent<C_KeyInput>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_ButtonSoundClip;
        //transform.Find("Offset_Grp").Find("BGM_Grp").GetComponent<AudioSource>().clip = m_BGMClip;
        //transform.Find("Offset_Grp").Find("BGM_Grp").GetComponent<AudioSource>().Play();
        //transform.Find("BGM_Grp").GetComponent<AudioSource>().clip = m_BGMClip;
        //transform.Find("BGM_Grp").GetComponent<AudioSource>().Play();

        ChangeButtonColor();
    }
    void Update()
    {
        if(m_IsMainMenu)
        {
            if(m_KeyInput.m_IsLeftKeyDown)
            {
                MoveIndex(-1);
            }
            if(m_KeyInput.m_IsRightKeyDown)
            {
                MoveIndex(1);
            }
        }
        if(m_KeyInput.m_IsMKeyDown)
        {
            ClickButton();
        }
    }

    public int MoveIndex(int moveNum = 1)
    {
        m_AudioSource.PlayOneShot(m_ButtonMoveSoundClip);
        m_ButtonIndex += moveNum;
        if(m_ButtonIndex >= m_Button.Count)
        {
            m_ButtonIndex = 0;
        }
        if(m_ButtonIndex < 0)
        {
            m_ButtonIndex = m_Button.Count - 1;
        }

        ChangeButtonColor();

        return m_ButtonIndex;
    }

    public void ChangeButtonColor()
    {
        for(int i = 0; i < m_Button.Count; ++i)
        {
            if(i == m_ButtonIndex)
            {
                m_Button[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                m_Button[i].GetComponent<Image>().color = Color.grey;
            }
        }
    }

    private void ClickButton()
    {
        bool isAction = false;
        m_AudioSource.PlayOneShot(m_ButtonSoundClip);
        if(m_IsMainMenu && isAction == false)
        {
            if(m_ButtonIndex == 0)
            {
				SceneManager.LoadScene ("Main");
            }
            else if(m_ButtonIndex == 1)
            {
                // Help
                m_IsMainMenu = false;
                m_IsHelp1 = true;
                SetActiveGrp(1);
            }
            else if(m_ButtonIndex == 2)
            {
                m_IsMainMenu = false;
                m_IsCredit = true;
                SetActiveGrp(3);
                // Credit
            }
            else if(m_ButtonIndex == 3)
            {
                Application.Quit();
                // Exit
            }
            isAction = true;
        }
        if(m_IsHelp1 && isAction == false)
        {
            m_IsHelp1 = false;
            m_IsHelp2 = true;
            SetActiveGrp(2);
            isAction = true;
        }
        if(m_IsHelp2 && isAction == false)
        {
            m_IsHelp2 = false;
            m_IsMainMenu = true;
            SetActiveGrp(0);
            isAction = true;
        }
        if(m_IsCredit && isAction == false)
        {
            m_IsCredit = false;
            m_IsMainMenu = true;
            SetActiveGrp(0);
            isAction = true;
        }
    }

    private void SetActiveGrp(int sceneIndex)
    {
        for(int i = 0; i < m_Scene.Count; ++i)
        {
            if(i == sceneIndex)
            {
                m_Scene[i].SetActive(true);
            }
            else
            {
                m_Scene[i].SetActive(false);
            }
        }
    }

    /*
    public int ShowPauseMenu(bool value)
    {
        gameObject.SetActive(value);
        if(value)
        {
            UpdateLevelIsClear();

            m_ButtonIndex = 0;
            ChangeButtonColor();
        }
        return m_ButtonIndex;
    }
    */
}
