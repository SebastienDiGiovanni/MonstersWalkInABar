using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cocktail
{
    public enum Glass
    {
        GLASS_TYPE_1,
        GLASS_TYPE_2,
        GLASS_TYPE_3,
        GLASS_TYPE_4,
        GLASS_COUNT
    }

    public enum Alcohol
    {
        ALCOHOL_TYPE_NONE,
        ALCOHOL_TYPE_1,
        ALCOHOL_TYPE_2,
        ALCOHOL_TYPE_3,
        ALCOHOL_TYPE_4,
        ALCOHOL_COUNT
    }

    public enum Fruit
    {
        FRUIT_TYPE_NONE,
        FRUIT_TYPE_1,
        FRUIT_TYPE_2,
        FRUIT_TYPE_3,
        FRUIT_TYPE_4,
        FRUIT_COUNT
    }

    public Glass m_glass;
    public Alcohol m_alcohol;
    public Fruit m_fruit;

    public Cocktail(Glass _glass, Alcohol _alcohol, Fruit _fruit)
    {
        m_glass = _glass;
        m_alcohol = _alcohol;
        m_fruit = _fruit;
    }
}

public class ClientManager : MonoBehaviour
{
    public int m_clientsCount = 6;
    public float m_firstSpawningDelay = 5.0F;
    public float m_spawningDelay = 30.0F;
    public GameObject m_clientPrefab;
    public Image m_mood;
    public GameManager m_gameManager;
    public List<Sprite> m_clientsSprites;
    public List<Sprite> m_glassSprites;
    public List<Sprite> m_alcoholSprites;
    public List<Sprite> m_fruitSprites;

    private List<int> m_freeSpawningLocations;
    private List<Client> m_clients;
    private float m_spawningTime;

    private AudioSource source;
    public AudioClip audioNewOrder;

    // Start is called before the first frame update
    void Start()
    {
        m_freeSpawningLocations = new List<int>();
        m_clients = new List<Client>();

        for (int i = 0; i < m_clientsCount; ++i)
        {
            m_freeSpawningLocations.Add(i);
        }

        m_spawningTime = Time.time + m_firstSpawningDelay;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_spawningTime)
        {
            m_spawningTime = Time.time + m_spawningDelay;

            if (m_freeSpawningLocations.Count > 0)
            {
                int randomIndex = Random.Range(0, m_freeSpawningLocations.Count);
                int clientIndex = m_freeSpawningLocations[randomIndex];

                // random cocktail
                Cocktail.Glass glass = (Cocktail.Glass)Random.Range((int)Cocktail.Glass.GLASS_TYPE_1, (int)Cocktail.Glass.GLASS_COUNT);
                Cocktail.Alcohol alcohol = (Cocktail.Alcohol)Random.Range((int)Cocktail.Alcohol.ALCOHOL_TYPE_1, (int)Cocktail.Alcohol.ALCOHOL_COUNT);
                Cocktail.Fruit fruit = (Cocktail.Fruit)Random.Range((int)Cocktail.Fruit.FRUIT_TYPE_1, (int)Cocktail.Fruit.FRUIT_COUNT);

                Cocktail cocktail = new Cocktail(glass, alcohol, fruit);

                m_freeSpawningLocations.RemoveAt(randomIndex);

                GameObject newClientGameObject = Instantiate(m_clientPrefab);
                newClientGameObject.transform.SetParent(transform.GetChild(clientIndex).transform);
                newClientGameObject.transform.localPosition = new Vector3(0.0F, 0.0F, 0.0F);

                // client sprite
                int randomSprite = Random.Range(0, m_clientsSprites.Count);
                newClientGameObject.GetComponent<SpriteRenderer>().sprite = m_clientsSprites[randomSprite];

                // glass sprite
                GameObject glassGameobject = newClientGameObject.transform.GetChild(0).GetChild(0).gameObject;
                if (glass == Cocktail.Glass.GLASS_TYPE_1)
                {
                    glassGameobject.GetComponent<SpriteRenderer>().sprite = m_glassSprites[0];
                }
                else if (glass == Cocktail.Glass.GLASS_TYPE_2)
                {
                    glassGameobject.GetComponent<SpriteRenderer>().sprite = m_glassSprites[1];
                }
                else if (glass == Cocktail.Glass.GLASS_TYPE_3)
                {
                    glassGameobject.GetComponent<SpriteRenderer>().sprite = m_glassSprites[2];
                }
                else if (glass == Cocktail.Glass.GLASS_TYPE_4)
                {
                    glassGameobject.GetComponent<SpriteRenderer>().sprite = m_glassSprites[3];
                }

                // alcohol sprite
                GameObject alcoholGameobject = newClientGameObject.transform.GetChild(0).GetChild(2).gameObject;
                if (alcohol == Cocktail.Alcohol.ALCOHOL_TYPE_1)
                {
                    alcoholGameobject.GetComponent<SpriteRenderer>().sprite = m_alcoholSprites[0];
                }
                else if (alcohol == Cocktail.Alcohol.ALCOHOL_TYPE_2)
                {
                    alcoholGameobject.GetComponent<SpriteRenderer>().sprite = m_alcoholSprites[1];
                }
                else if (alcohol == Cocktail.Alcohol.ALCOHOL_TYPE_3)
                {
                    alcoholGameobject.GetComponent<SpriteRenderer>().sprite = m_alcoholSprites[2];
                }
                else if (alcohol == Cocktail.Alcohol.ALCOHOL_TYPE_4)
                {
                    alcoholGameobject.GetComponent<SpriteRenderer>().sprite = m_alcoholSprites[3];
                }

                // fruit sprite
                GameObject fruitGameobject = newClientGameObject.transform.GetChild(0).GetChild(1).gameObject;
                if (fruit == Cocktail.Fruit.FRUIT_TYPE_1)
                {
                    fruitGameobject.GetComponent<SpriteRenderer>().sprite = m_fruitSprites[0];
                }
                else if (fruit == Cocktail.Fruit.FRUIT_TYPE_2)
                {
                    fruitGameobject.GetComponent<SpriteRenderer>().sprite = m_fruitSprites[1];
                }
                else if (fruit == Cocktail.Fruit.FRUIT_TYPE_3)
                {
                    fruitGameobject.GetComponent<SpriteRenderer>().sprite = m_fruitSprites[2];
                }
                else if (fruit == Cocktail.Fruit.FRUIT_TYPE_4)
                {
                    fruitGameobject.GetComponent<SpriteRenderer>().sprite = m_fruitSprites[3];
                }

                Client clientScript = newClientGameObject.GetComponent<Client>();
                clientScript.Init(clientIndex, cocktail, this, m_mood);
                m_clients.Add(clientScript);

                source.PlayOneShot(audioNewOrder);
            }
        }
    }

    public void Quit(int _clientIndex, GameObject _gameObject)
    {
        m_freeSpawningLocations.Add(_clientIndex);

        for (int i = 0; i < m_clients.Count; ++i)
        {
            if (m_clients[i].getIndex() == _clientIndex)
            {
                m_clients.RemoveAt(i);
            }
        }

        Destroy(_gameObject);

        if (Time.time > m_spawningTime)
        {
            m_spawningTime += m_spawningDelay;
        }
    }
}
