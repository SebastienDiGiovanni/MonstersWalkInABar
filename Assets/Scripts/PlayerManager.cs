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

    private float m_pickupCooldownSeconds = 1.0f;
    private float m_pickupTimer = 0.0f;

    private int m_playerIndex;

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

        m_pickupTimer += Time.deltaTime;

        Debug.Log("m_currentlyCollidingWith: " + (m_currentlyCollidingWith ? m_currentlyCollidingWith.gameObject.name : "null"));

        if (Input.GetButton("Valid" + playerIndex) && m_currentlyCollidingWith && (m_pickupTimer > m_pickupCooldownSeconds))
        {
            Debug.Log("m_currentObjectCarried: " + (m_currentObjectCarried ? m_currentObjectCarried.name : "null"));
            if (!m_currentObjectCarried) // pick up the object only if we are not carrying anything
            {
                // m_currentlyCollidingWith contains whatever we want to pick right now
                m_currentObjectCarried = m_currentlyCollidingWith.gameObject;

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

                m_pickupTimer = 0.0f;
            }
            else
            {
                // if we are carrying something, we can put it down
                
                // if it is an alcohol or fruit -- only put it into the glass or the trash
                if (m_currentObjectCarried.tag.Contains("Fruit") || m_currentObjectCarried.tag.Contains("Alcohol"))
                {
                    // drop it into the glass if we are colliding with a glass
                    if (m_currentlyCollidingWith && m_currentlyCollidingWith.gameObject.tag.Contains("Glass"))
                    {
                        Debug.Log("Dropping into the glass: " + m_currentObjectCarried.name);
                        Glass g = m_currentlyCollidingWith.GetComponent<Glass>();
                        m_currentObjectCarried.transform.SetParent(m_currentlyCollidingWith.gameObject.transform);
                        
                        m_currentObjectCarried.GetComponent<SpriteRenderer>().sortingLayerName = "BetweenBarAndPlayer";
                        m_currentObjectCarried.GetComponent<SpriteRenderer>().sortingOrder = 2;

                        Alcohol a = m_currentObjectCarried.GetComponent<Alcohol>();
                        if (a)
                        {
                            m_currentObjectCarried.transform.localPosition = new Vector3(0, 0.5f, 0);
                            g.addAlcohol(a);
                        }
                        Fruit f = m_currentObjectCarried.GetComponent<Fruit>();
                        if (f)
                        {
                            m_currentObjectCarried.transform.localPosition = Vector3.zero;
                            g.addFruit(f);
                        }

                        m_currentObjectCarried = null;
                        m_pickupTimer = 0.0f;
                    }
                }

                if (m_currentObjectCarried.tag.Contains("Glass"))
                {
                    // TODO:
                        // Check if we're colliding with the Client
                        // Then drop the glass on the 
                        // The client will automatically evaluate the drink
                        // Maybe launch the evaluation from here
                }

                if (m_currentlyCollidingWith && m_currentlyCollidingWith.gameObject.tag.Contains("Trash"))
                {
                    Debug.Log("Dropping into the trash: " + m_currentObjectCarried.name);
                    Destroy(m_currentObjectCarried);
                    m_currentObjectCarried = null;
                    m_pickupTimer = 0.0f;
                }
            }
        }
    }
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Pickable") && !m_currentlyCollidingWith)
        {
            //Debug.Log("collided with " + other.tag + " " + other.gameObject.name);
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
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Contains("Pickable") && !m_currentlyCollidingWith)
        {
            //Debug.Log("collided with " + other.tag + " " + other.gameObject.name);
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
            m_currentlyCollidingWith = null;
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