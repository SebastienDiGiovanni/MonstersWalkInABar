using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float m_moodDecreaseAmount;

    private List<GameObject> m_players;

    private float m_gameTimer;
    private float m_mood;
    private float m_moodDecreaseTime;

    private bool m_gameOver;

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



        // TODO REMOVE
        SpawnPlayer(0, 1);
        SpawnPlayer(1, 25);
        SpawnPlayer(2, 26);
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

            if (m_gameTimer > m_maxTime)
            {
                m_gameTimer = m_maxTime;
            }

            int seconds = (int)m_gameTimer;
            int minutes = seconds / 60;
            seconds = seconds % 60;
            m_timerText.text = minutes.ToString() + ":" + (seconds > 9 ? seconds.ToString() : "0" + seconds.ToString());

            m_mood = m_moodBar.GetComponent<RectTransform>().localScale.x;

            bool loose = m_mood < Mathf.Epsilon;
            if (Mathf.Abs(m_gameTimer - m_maxTime) < Mathf.Epsilon // end of time
                || loose)
            {
                m_gameOver = true;
                if (loose)
                {
                    // loose
                }
                else
                {
                    // win
                }
            }
        }
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
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_YellowPlayerFront, m_YellowPlayerBack, m_YellowPlayerLeft, m_YellowPlayerRight);
                break;

            case 3:
                newPlayer.GetComponent<PlayerManager>().SetSprites(m_YellowPlayerFront, m_YellowPlayerBack, m_YellowPlayerLeft, m_YellowPlayerRight);
                break;
        }
        m_players.Add(newPlayer);
    }

    public void IncreaseMood(float _amount)
    {
        float oldValue = m_mood;
        m_mood -= _amount;
        m_moodBar.GetComponent<RectTransform>().localScale = new Vector3(m_mood, 1.0F, 1.0F);

        if (oldValue > 0.1 && m_mood <= 0.1)
        {
            m_moodBar.GetComponent<Image>().color = new Color(1.0F, 0.0F, 0.0F);
        }
    }

    public void DecreaseMood(float _amount)
    {
        float oldValue = m_mood;
        m_mood -= _amount;
        m_moodBar.GetComponent<RectTransform>().localScale = new Vector3(m_mood, 1.0F, 1.0F);

        if (oldValue > 0.1 && m_mood <= 0.1)
        {
            m_moodBar.GetComponent<Image>().color = new Color(1.0F, 0.0F, 0.0F);
        }
    }
}
