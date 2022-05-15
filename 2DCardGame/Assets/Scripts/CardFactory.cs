using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardFactory
{
    private static CardController controller = null;
    private static GameObject cardPrefab = null;

    /* store the key: string of cards and Value: dsct card. */
    private static Dictionary<string, Cards> graph = null;

    public static void SetCardController(CardController _controller)
    {
        controller = _controller;
    }

    /**
     * Create the card by reading the recipts and card collection.
     */
    public static GameObject SpawnCard(CardController cards)
    {
        Initialize();
        Cards card;

        if (graph.TryGetValue(cards.ToString(), out card))
        {
            //TODO
            GameObject cardObject = GameObject.Instantiate(cardPrefab);
            return cardObject;
        }
        else
            return null;
    }

    /**
     * Set up the dictionary, takes recipes as content.
     */
    private static void Initialize()
    {
        if (graph != null)
            return;

        graph = new Dictionary<string, Cards>();

        if (cardPrefab == null)
            cardPrefab = Resources.Load("Card") as GameObject;

        List<CardRecipes> recipes = controller.GetCardRecipes();

        foreach (CardRecipes recipe in recipes) 
        {
            List<Cards> sources = new List<Cards>(recipe.GetSources());
            sources.Sort();
            string key = ""; 
            for(int i = 0; i < sources.Count - 1; ++i)
            {
                key += sources[i].ToString() + "+";
            }
            key += sources[sources.Count - 1];

            graph.Add(key, recipe.GetTarget());
            // Debug.Log("Add Recipe Entry: " + key + " " + recipe.GetTarget().ToString());
        }
    }
}
