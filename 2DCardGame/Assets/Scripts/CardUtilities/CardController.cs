using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cards
{
    Tree,
    IronOre,
    MagicOre,
    Wood,
    Iron,
    MagicStone,
    Villager,
}


public class CardController : MonoBehaviour
{
    private static CardController controller = null;

    /* Recipes are given from Editor. */
    public List<CardRecipe> recipes;
    public List<PrefabPath> paths;

    private void Awake()
    {
        if (controller && controller != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            controller = this;
        }
    }

    public List<CardRecipe> GetCardRecipes()
    {
        return recipes;
    }

    public static CardController GetInstance()
    {
        return controller;
    }
}
