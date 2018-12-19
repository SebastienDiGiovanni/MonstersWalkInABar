﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float m_speed;

    private Sprite m_front;
    private Sprite m_back;
    private Sprite m_left;
    private Sprite m_right;

    private Rigidbody2D m_rigidbody2D;        //The Rigidbody2D component attached to this object.
    private SpriteRenderer m_spriteRenderer;

    private Collider2D m_currentlyCollidingWith;
    private GameObject m_currentObjectCarried;

    private int m_playerIndex;

    private GameObject m_client;

    public GameManager m_gameManager;

    private bool m_needToClearBubbleFeedback;
    private float m_bubbleFeedbackStopTimer;

    private bool m_nextToTrashcan;

    // Start is called before the first frame update
    void Start()
    {
        //Get a component reference to this object's Rigidbody2D
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        if (m_spriteRenderer == null)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
        m_currentObjectCarried = null;

        m_needToClearBubbleFeedback = false;

        m_nextToTrashcan = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        string playerIndex = m_playerIndex.ToString();
        float moveHorizontal = Input.GetAxis("Horizontal" + playerIndex);
        float moveVertical = Input.GetAxis("Vertical" + playerIndex);

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        if (movement.sqrMagnitude > float.Epsilon)
        {
            m_rigidbody2D.transform.Translate(movement * m_speed);

            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // mainly horizontal movement
                if (movement.x < 0.0F)
                {
                    // moving on the left
                    m_spriteRenderer.sprite = m_left;
                }
                else
                {
                    // moving on the right
                    m_spriteRenderer.sprite = m_right;
                }
            }
            else
            {
                // mainly vertical movement
                if (movement.y < 0)
                {
                    // moving in front
                    m_spriteRenderer.sprite = m_front;
                }
                else
                {
                    // moving backward
                    m_spriteRenderer.sprite = m_back;
                }
            }
        }

        if (Input.GetButtonDown("Valid" + playerIndex))
        {
            if (m_currentlyCollidingWith)
            {
                if (!m_currentObjectCarried) // pick up the object only if we are not carrying anything
                {
                    // m_currentlyCollidingWith contains whatever we want to pick right now
                    m_currentObjectCarried = GameObject.Instantiate(m_currentlyCollidingWith.gameObject);

                    Glass g = m_currentObjectCarried.GetComponent<Glass>();
                    if (g)
                    {
                        g.setSelected(true);
                    }
                    Alcohol a = m_currentObjectCarried.GetComponent<Alcohol>();
                    if (a)
                    {
                        a.setSelected(true);
                    }
                    Fruit f = m_currentObjectCarried.GetComponent<Fruit>();
                    if (f)
                    {
                        f.setSelected(true);
                    }

                    // disable the collider on the picked object and move it over the head.
                    m_currentObjectCarried.GetComponent<BoxCollider2D>().enabled = false;
                    m_currentObjectCarried.transform.SetParent(gameObject.transform);
                    m_currentObjectCarried.transform.localPosition = new Vector3(0, 2, 0);
                    m_currentObjectCarried.GetComponent<SpriteRenderer>().sortingLayerName = "BetweenBarAndTrashcan";
                }
                else if ((m_currentObjectCarried.tag.Contains("Fruit") || m_currentObjectCarried.tag.Contains("Alcohol"))
                    && m_currentlyCollidingWith.gameObject.tag.Contains("Glass"))
                {
                    // if we are carrying an alcohol or fruit, we can put it into the glass
                    Debug.Log("Dropping into the glass: " + m_currentObjectCarried.name);
                    Glass g = m_currentlyCollidingWith.GetComponent<Glass>();
                    m_currentObjectCarried.transform.SetParent(m_currentlyCollidingWith.gameObject.transform);

                    m_currentObjectCarried.GetComponent<SpriteRenderer>().sortingLayerName = "BetweenBarAndPlayer";
                    m_currentObjectCarried.GetComponent<SpriteRenderer>().sortingOrder = 2;

                    Alcohol a = m_currentObjectCarried.GetComponent<Alcohol>();
                    if (a)
                    {
                        Debug.Log("DROPPING ALCOHOL : " + a.m_alcohol);
                        m_currentObjectCarried.transform.localPosition = new Vector3(0, 0.5f, 0);
                        g.addAlcohol(a);
                    }
                    Fruit f = m_currentObjectCarried.GetComponent<Fruit>();
                    if (f)
                    {
                        Debug.Log("DROPPING FRUIT : " + f.m_fruit);
                        m_currentObjectCarried.transform.localPosition = Vector3.zero;
                        g.addFruit(f);
                    }

                    m_currentObjectCarried = null;
                }
            }
            else if (m_nextToTrashcan && m_currentObjectCarried)
            {
                Destroy(m_currentObjectCarried);
                m_currentObjectCarried = null;
            }
            else if (m_client && m_currentObjectCarried)
            {
                if (m_currentObjectCarried.tag.Contains("Glass"))
                {
                    // a glass
                    Glass glass = m_currentObjectCarried.GetComponent<Glass>();
                    if (glass)
                    {
                        Client client = m_client.GetComponent<Client>();
                        Cocktail wantedCocktail = client.GetWantedCocktail();

                        Debug.Log(glass.m_glass + " : " + wantedCocktail.m_glass);
                        Debug.Log(glass.m_fruit + " : " + wantedCocktail.m_fruit);
                        Debug.Log(glass.m_alcohol + " : " + wantedCocktail.m_alcohol);

                        if (glass.m_glass == wantedCocktail.m_glass
                            && glass.m_fruit == wantedCocktail.m_fruit
                            && glass.m_alcohol == wantedCocktail.m_alcohol)
                        {
                            // right command
                            ClientHappy();
                        }
                        else
                        {
                            ClientNotHappy();
                        }
                    }
                }
                else
                {
                    // not a glass
                    ClientNotHappy();
                }
            }

        }

        if (m_needToClearBubbleFeedback && Time.time > m_bubbleFeedbackStopTimer)
        {
            m_client.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
            m_client.transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer>().enabled = false;
            m_needToClearBubbleFeedback = false;
        }
    }

    public void SetGameManager(GameManager _gameManager)
    {
        m_gameManager = _gameManager;
    }

    public void ClientHappy()
    {
        m_gameManager.IncreaseMood(0.1F);
        m_client.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
        m_needToClearBubbleFeedback = true;
        m_bubbleFeedbackStopTimer = Time.time + 1.0F;
        Debug.Log("Client happy");
    }

    public void ClientNotHappy()
    {
        m_gameManager.DecreaseMood(0.1F);
        m_client.transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer>().enabled = true;
        m_needToClearBubbleFeedback = true;
        m_bubbleFeedbackStopTimer = Time.time + 1.0F;
        Debug.Log("Client not happy");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Pickable") && !m_currentlyCollidingWith)
        {
            //Debug.Log("collided with " + other.tag + " " + other.gameObject.name);
            Debug.Log("TES");
            m_currentlyCollidingWith = other;
            GameObject go = other.gameObject;
            Glass g = go.GetComponent<Glass>();
            if (g)
            {
                g.setSelected(true);
            }
            Alcohol a = go.GetComponent<Alcohol>();
            if (a)
            {
                a.setSelected(true);
            }
            Fruit f  = go.GetComponent<Fruit>();
            if (f)
            {
                f.setSelected(true);
            }
        }
        else if (other.tag.Contains("Client") && !m_client)
        {
            Debug.Log("New client !");
            m_client = other.gameObject;
        }

        if (other.tag.Contains("Trash"))
        {
            m_nextToTrashcan = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Contains("Pickable") && !m_currentlyCollidingWith)
        {
            //Debug.Log("collided with " + other.tag + " " + other.gameObject.name);
            Debug.Log("TES");
            m_currentlyCollidingWith = other;
            GameObject go = other.gameObject;
            Glass g = go.GetComponent<Glass>();
            if (g)
            {
                g.setSelected(true);
            }
            Alcohol a = go.GetComponent<Alcohol>();
            if (a)
            {
                a.setSelected(true);
            }
            Fruit f = go.GetComponent<Fruit>();
            if (f)
            {
                f.setSelected(true);
            }
        }
        else if (other.tag.Contains("Client") && !m_client)
        {
            m_client = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        Glass g = go.GetComponent<Glass>();
        if (g)
        {
            g.setSelected(false);
        }
        Alcohol a = go.GetComponent<Alcohol>();
        if (a)
        {
            a.setSelected(false);
        }
        Fruit f = go.GetComponent<Fruit>();
        if (f)
        {
            f.setSelected(false);
        }
        if (m_currentlyCollidingWith == other)
        {
            //Debug.Log("collided with " + other.tag + " " + other.gameObject.name);
            Debug.Log("NO");
            m_currentlyCollidingWith = null;
        }

        if (other.tag.Contains("Client") && m_client)
        {
            Debug.Log("Remove client !");
            m_client = null;
        }

        if (other.tag.Contains("Trash"))
        {
            m_nextToTrashcan = false;
        }
    }

    public void SetSprites(Sprite _front, Sprite _back, Sprite _left, Sprite _right)
    {
        m_front = _front;
        m_back = _back;
        m_left = _left;
        m_right = _right;

        if (m_spriteRenderer == null)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
        m_spriteRenderer.sprite = m_front;
    }

    public void SetPlayerIndex(int _index)
    {
        m_playerIndex = _index;
    }

    public GameObject getCurrentObjectCarried()
    {
        return m_currentObjectCarried;
    }
}