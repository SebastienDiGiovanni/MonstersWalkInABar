using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    private int m_index;

    private Cocktail m_cocktail;

    private ClientManager m_clientManager;
    private Image m_mood;

    private bool m_needToQuit;
    private float m_quitTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_needToQuit = false;
    }

    void Update()
    {
        if (m_needToQuit && Time.time > m_quitTimer)
        {
            m_clientManager.Quit(m_index, transform.gameObject);
        }
    }

    
    public void Init(int _index, Cocktail _cocktail, ClientManager _clientManager, Image _mood)
    {
        m_index = _index;
        m_cocktail = _cocktail;
        m_clientManager = _clientManager;
        m_mood = _mood;
    }

    public Cocktail GetWantedCocktail()
    {
        return m_cocktail;
    }

    public void Quit(bool _happy)
    {
        m_needToQuit = true;
        m_quitTimer = Time.time + 1.0F;

        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().enabled = false;

        if (_happy)
        {
            transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public int getIndex()
    {
        return m_index;
    }
}
