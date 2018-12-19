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
}
