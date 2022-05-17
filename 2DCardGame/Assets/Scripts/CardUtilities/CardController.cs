using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cards
{
    A, B, C
}


public class CardController : MonoBehaviour
{
    static private CardController controller = null;

    /* Recipes are given from Editor. */
    [SerializeField] private List<CardRecipes> recipes;

    private void Awake()
    {
        if (controller && controller != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            controller = this;
            CardFactory.SetCardController(controller);
        }
    }

    public List<CardRecipes> GetCardRecipes()
    {
        return recipes;
    }

    public static CardController Instantiate()
    {
        return controller;
    }
}
