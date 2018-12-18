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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
            PlayerManager playerScript = playerGameObject.GetComponent<PlayerManager>();
            GameObject objectCarried = playerScript.getCurrentObjectCarried();

            if (objectCarried != null)
            {
                // player is bringing something to the client
                if (objectCarried.tag.Contains("Glass"))
                {
                    // a glass
                    Glass glass = objectCarried.GetComponent<Glass>();
                    if (glass)
                    {
                        if (glass.m_glass == m_cocktail.m_glass
                            && glass.m_fruit == m_cocktail.m_fruit
                            && glass.m_alcohol == m_cocktail.m_alcohol)
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
    }

    public void ClientHappy()
    {
        m_clientManager.m_gameManager.GetComponent<GameManager>().IncreaseMood(0.1F);
    }

    public void ClientNotHappy()
    {
        m_clientManager.m_gameManager.GetComponent<GameManager>().DecreaseMood(0.1F);
    }
}
