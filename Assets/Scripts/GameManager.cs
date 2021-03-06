﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Text m_timerText;
    public int m_maxTime;
    public Image m_moodBar;
    public GameObject m_playerPrefab;

    public Sprite m_YellowPlayerFront;
    public Sprite m_YellowPlayerBack;
    public Sprite m_YellowPlayerRight;
    public Sprite m_YellowPlayerLeft;

    public Sprite m_BluePlayerFront;
    public Sprite m_BluePlayerBack;
    public Sprite m_BluePlayerRight;
    public Sprite m_BluePlayerLeft;

    public Sprite m_PinkPlayerFront;
    public Sprite m_PinkPlayerBack;
    public Sprite m_PinkPlayerRight;
    public Sprite m_PinkPlayerLeft;

    public Sprite m_GreenPlayerFront;
    public Sprite m_GreenPlayerBack;
    public Sprite m_GreenPlayerRight;
    public Sprite m_GreenPlayerLeft;

    public float m_moodDecreaseAmount;

    public ClientManager m_clientManager;

    public Sprite m_win;
    public Sprite m_loose;

    private List<GameObject> m_players;

    private float m_gameTimer;
    private float m_mood;
    private float m_moodDecreaseTime;

    private bool m_gameOver;

    private AudioSource source;
    public AudioClip audioWin, audioLoose;

    // Start is called before the first frame update
    void Start()
    {
        m_players = new List<GameObject>();

        m_gameTimer = 0.0F;
        m_gameOver = false;

        m_moodDecreaseTime = Time.time + 1.0F;

        if (StaticData.m_playersCount > 4)
        {
            StaticData.m_playersCount = 4;
        }

        // TODO REMOVE COMMENT
        /*string[] joystickNames = Input.GetJoystickNames();
        int joystickIndex = 1;
        bool playerSpawned = false;
 
        for (int playerIndex = 1; playerIndex <= StaticData.m_playersCount; ++playerIndex)
        {
            playerSpawned = false;
            while (joystickIndex <= joystickNames.Length && !playerSpawned)
            {
                string joystickName = joystickNames[joystickIndex - 1];
                if (joystickName.Length > 0)
                {
                    SpawnPlayer(playerIndex - 1, joystickIndex);
                    playerSpawned = true;
                }
                ++joystickIndex;
            }
        }*/



        SpawnPlayer(0, 1);
        SpawnPlayer(1, 2);
        SpawnPlayer(2, 3);
        SpawnPlayer(3, 4);



        /*Debug.Log("joysticks");
        
        for (int i = 0; i < joystickNames.Length; ++i)
        { 
            if (joystickNames[i].Length == 0)
            {
                Debug.Log("EMPTY");
            }
            else
            {
                Debug.Log(joystickNames[i]);
            }
        }*/

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_gameOver)
        {
            List<int> topBottomplayersIndex = new List<int>();
            List<float> yPositions = new List<float>();

            for (int i = 0; i < m_players.Count; ++i)
            {
                float yPos = m_players[i].transform.position.y;
                int index = 0;

                while (index < yPositions.Count && yPositions[index] > yPos)
                {
                    ++index;
                }

                topBottomplayersIndex.Insert(index, i + 1);
                yPositions.Insert(index, yPos);
            }

            int counter = 1;
            for (int i = 0; i < topBottomplayersIndex.Count; ++i)
            {
                m_players[topBottomplayersIndex[i]-1].GetComponent<SpriteRenderer>().sortingLayerName = "PlayerLayer" + counter.ToString();
                ++counter;
            }


            if (Time.time > m_moodDecreaseTime)
            {
                DecreaseMood(m_moodDecreaseAmount);
                m_moodDecreaseTime = Time.time + 1.0F;
            }

            m_gameTimer += Time.deltaTime;

            float playTime = m_maxTime - m_gameTimer;

            if (playTime < 0.0F)
            {
                playTime = 0.0F;
            }

            int seconds = (int)playTime;
            int minutes = seconds / 60;
            seconds = seconds % 60;
            m_timerText.text = minutes.ToString() + ":" + (seconds > 9 ? seconds.ToString() : "0" + seconds.ToString());

            m_mood = m_moodBar.GetComponent<RectTransform>().localScale.x;

            bool loose = m_mood < Mathf.Epsilon;
            if (Mathf.Abs(playTime) < Mathf.Epsilon // end of time
                || loose)
            {
                m_gameOver = true;

                for (int i = 0; i < m_players.Count; ++i)
                {
                    Destroy(m_players[i]);
                }

                m_clientManager.DestroyEveryClient();

                if (m_mood < 0.3F)
                {
                    loose = true;
                }

                if (loose)
                {
                    transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = m_loose;
                    source.PlayOneShot(audioLoose);
                }
                else
                {
                    transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = m_win;
                    source.PlayOneShot(audioWin);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Valid2"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public bool IsGameOver()
    {
        return m_gameOver;
    }

    public void SpawnPlayer(int _spawningLocationIndex, int _joystickIndex)
    {
        Debug.Log("Use joystick index: " + _joystickIndex);
        GameObject newPlayer = Instantiate(m_playerPrefab);
        newPlayer.transform.position = transform.GetChild(_spawningLocationIndex).position;
        newPlayer.GetComponent<PlayerManager>().SetPlayerIndex(_joystickIndex);
        switch (_spawningLocationIndex)
        {
            case 0:
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_YellowPlayerFront, m_YellowPlayerBack, m_YellowPlayerLeft, m_YellowPlayerRight);
                break;

            case 1:
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_BluePlayerFront, m_BluePlayerBack, m_BluePlayerLeft, m_BluePlayerRight);
                break;

            case 2:
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_PinkPlayerFront, m_PinkPlayerBack, m_PinkPlayerLeft, m_PinkPlayerRight);
                break;

            case 3:
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_GreenPlayerFront, m_GreenPlayerBack, m_GreenPlayerLeft, m_GreenPlayerRight);
                break;
        }
        m_players.Add(newPlayer);
        newPlayer.GetComponent<PlayerManager>().SetManagers(GetComponent<GameManager>(), m_clientManager);
    }

    public void IncreaseMood(float _amount)
    {
        float oldValue = m_mood;
        m_mood += _amount;
        if (m_mood > 1.0F)
        {
            m_mood = 1.0F;
        }
        m_moodBar.GetComponent<RectTransform>().localScale = new Vector3(m_mood, 1.0F, 1.0F);

        if (oldValue < 0.3 && m_mood >= 0.3)
        {
            m_moodBar.GetComponent<Image>().color = new Color(0.0F, 1.0F, 0.0F);
        }
    }

    public void DecreaseMood(float _amount)
    {
        float oldValue = m_mood;
        m_mood -= _amount;
        if (m_mood < 0.0F)
        {
            m_mood = 0.0F;
        }
        m_moodBar.GetComponent<RectTransform>().localScale = new Vector3(m_mood, 1.0F, 1.0F);

        if (oldValue >= 0.3 && m_mood < 0.3)
        {
            m_moodBar.GetComponent<Image>().color = new Color(1.0F, 0.0F, 0.0F);
        }
    }
}
