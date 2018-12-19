using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public int m_fruitCount = 6;
    public float m_firstSpawningDelay = 5.0F;
    public float m_spawningDelay = 30.0F;
    public List<GameObject> m_fruitPrefabs;

    private List<int> m_freeSpawningLocations;
    private List<Fruit> m_fruits;
    private float m_spawningTime;

    // Start is called before the first frame update
    void Start()
    {
        m_freeSpawningLocations = new List<int>(); ;
        m_fruits = new List<Fruit>();

        for (int i = 0; i < m_fruitCount; ++i)
        {
            m_freeSpawningLocations.Add(i);
        }

        m_spawningTime = Time.time + m_firstSpawningDelay;
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
                int index = m_freeSpawningLocations[randomIndex];

                m_freeSpawningLocations.RemoveAt(randomIndex);

                GameObject newGameObject = Instantiate(m_fruitPrefabs[index]);
                newGameObject.transform.position = transform.GetChild(index).position;
                Fruit script = newGameObject.GetComponent<Fruit>();
                m_fruits.Add(script);
            }
        }
    }
}