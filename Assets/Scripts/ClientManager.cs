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

public class CocktailDB
{
    public List<Cocktail> m_cocktails;

    public CocktailDB()
    {
        m_cocktails = new List<Cocktail>();
        m_cocktails.Add(new Cocktail(Cocktail.Glass.GLASS_TYPE_1, Cocktail.Alcohol.ALCOHOL_TYPE_1, Cocktail.Fruit.FRUIT_TYPE_1));
        m_cocktails.Add(new Cocktail(Cocktail.Glass.GLASS_TYPE_2, Cocktail.Alcohol.ALCOHOL_TYPE_2, Cocktail.Fruit.FRUIT_TYPE_2));
        m_cocktails.Add(new Cocktail(Cocktail.Glass.GLASS_TYPE_3, Cocktail.Alcohol.ALCOHOL_TYPE_3, Cocktail.Fruit.FRUIT_TYPE_3));
    }
}

public class ClientManager : MonoBehaviour
{
    public int m_clientsCount = 6;
    public float m_firstSpawningDelay = 5.0F;
    public float m_spawningDelay = 30.0F;
    public List<GameObject> m_clientsPrefabs;
    public Image m_mood;
    public GameManager m_gameManager;

    private List<int> m_freeSpawningLocations;
    private List<Client> m_clients;
    private float m_spawningTime;
    private CocktailDB m_cocktailDB;

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

        m_cocktailDB = new CocktailDB();
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

                List<Cocktail> cocktails = m_cocktailDB.m_cocktails;
                int cocktailIndex = Random.Range(0, cocktails.Count);
                Cocktail cocktail = cocktails[cocktailIndex];

                m_freeSpawningLocations.RemoveAt(randomIndex);

                GameObject newClientGameObject = Instantiate(m_clientsPrefabs[clientIndex]);
                newClientGameObject.transform.position = transform.GetChild(clientIndex).position;
                Client clientScript = newClientGameObject.GetComponent<Client>();
                clientScript.Init(clientIndex, cocktail, this, m_mood);
                m_clients.Add(clientScript);
            }
        }
    }
}
