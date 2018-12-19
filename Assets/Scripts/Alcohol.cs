using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcohol : MonoBehaviour
{
    public Sprite selectedModeSprite;
    public Sprite notSelectedModeSprite;

    public Cocktail.Alcohol m_alcohol;
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

    public Cocktail.Alcohol getType()
    {
        return m_alcohol;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
