using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public int m_sceneToLoad;
    public float m_delay;

    private float m_timer;

    void Start()
    {
        m_timer = Time.time + m_delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Valid2")
            && Time.time > m_timer)
        {
            SceneManager.LoadScene(m_sceneToLoad);
        }
    }
}
