using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: cannot pick an empty glass

public class Glass : MonoBehaviour
{
    public Cocktail.Glass m_glass;
    public Cocktail.Alcohol m_alcohol;
    public Cocktail.Fruit m_fruit;

    public Sprite selectedModeSprite;
    public Sprite notSelectedModeSprite;

    public void setSelected(bool selected)
    {
        if (selected)
        {
            this.GetComponent<SpriteRenderer>().sprite = selectedModeSprite;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = notSelectedModeSprite;
        }
    }

    public void addAlcohol(Alcohol a)
    {
        m_alcohol = a.getType();
        Debug.Log("Add alcohol " + m_alcohol);
    }

    public void addFruit(Fruit f)
    {
        m_fruit = f.getType();
        Debug.Log("Add alcohol " + m_fruit);
    }

    public Cocktail.Glass getType()
    {
        return m_glass;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
