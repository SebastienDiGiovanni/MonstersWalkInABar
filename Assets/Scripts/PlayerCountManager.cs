using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCountManager : MonoBehaviour
{
    public Text m_leftArrow;
    public Text m_rightArrow;

    public float threshold = 0.5F;

    private bool m_ignoreLeft;
    private bool m_ignoreRight;

    // Start is called before the first frame update
    void Start()
    {
        StaticData.m_playersCount = int.Parse(GetComponent<Text>().text);

        m_ignoreLeft = false;
        m_ignoreRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput <= -threshold)
        {
            if (!m_ignoreLeft && StaticData.m_playersCount != 1)
            {
                if (StaticData.m_playersCount == 4)
                {
                    m_rightArrow.enabled = true;
                }
                StaticData.m_playersCount = StaticData.m_playersCount - 1;
                GetComponent<Text>().text = StaticData.m_playersCount.ToString();
                if (StaticData.m_playersCount == 1)
                {
                    m_leftArrow.enabled = false;
                }
                m_ignoreLeft = true;
            }
        }
        else if (-threshold < horizontalInput && horizontalInput < 0)
        {
            if (m_ignoreLeft)
            {
                m_ignoreLeft = false;
            }
        }
        else if (0 <= horizontalInput && horizontalInput < threshold)
        {
            if (m_ignoreRight)
            {
                m_ignoreRight = false;
            }
        }
        else if (threshold <= horizontalInput)
        {
            if (!m_ignoreRight && StaticData.m_playersCount != 4)
            {
                if (StaticData.m_playersCount == 1)
                {
                    m_leftArrow.enabled = true;
                }
                StaticData.m_playersCount = StaticData.m_playersCount + 1;
                GetComponent<Text>().text = StaticData.m_playersCount.ToString();
                if (StaticData.m_playersCount == 4)
                {
                    m_rightArrow.enabled = false;
                }
                m_ignoreRight = true;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
