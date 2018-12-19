using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public Sprite selectedModeSprite;
    public Sprite notSelectedModeSprite;

    public Cocktail.Fruit m_fruit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Cocktail.Fruit getType()
    {
        return m_fruit;
    }

    public void setSelected(bool selected)
    {
        if (selected)
        {
            this.GetComponent<SpriteRenderer>().sprite = selectedModeSprite;
        } else
        {
            this.GetComponent<SpriteRenderer>().sprite = notSelectedModeSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
            PlayerManager playerScript = playerGameObject.GetComponent<PlayerManager>();
        }
    }
}
