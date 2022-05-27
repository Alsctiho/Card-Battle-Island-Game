using System;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory: MonoBehaviour
{
    private static CardFactory cardFactory = null;

    private CardController controller = null;

    /* store the key: string of cards and Value: dsct cards & time required. */
    private Dictionary<string, CardRecipe> graph = null;
    private Dictionary<Cards, GameObject> paths = null;

    /* store the time information */
    private Dictionary<CardCollection, float> collectionTimePairs = new();
    List<CardCollection> removingCollections = new();
    List<CardCollection> pausingCollections = new();

    private void Awake()
    {
        if(cardFactory && cardFactory != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            cardFactory = this;
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        removingCollections.Clear();

        List<CardCollection> listOfCollections = new List<CardCollection>(collectionTimePairs.Keys);
        foreach(CardCollection cardCollection in listOfCollections)
        {
            if (pausingCollections.Contains(cardCollection))
                continue;

            float timeLeft = collectionTimePairs[cardCollection] - Time.deltaTime;

            if(timeLeft <= 0.0f)
            {
                KeepSpawnCard(cardCollection);
                removingCollections.Add(cardCollection);
            }
            else
            {
                collectionTimePairs[cardCollection] = timeLeft;
            }
        }

        foreach(CardCollection collection in removingCollections)
        {
            collectionTimePairs.Remove(collection);
        }
    }

    public void RequestSpawnCard(CardCollection cards)
    {
        if (pausingCollections.Contains(cards))
        {
            pausingCollections.Remove(cards);
        }

        if (collectionTimePairs.ContainsKey(cards))
            return;

        if (graph.TryGetValue(cards.ToString(), out CardRecipe recipe)) 
        { 
            // Register the request.
            collectionTimePairs.Add(cards, recipe.GetTime());
        }
    }
    
    public void ClearCollection(CardCollection collection)
    {
        collectionTimePairs.Remove(collection);
    }

    /**
     * Create the card by reading the recipes and card collection.
     */
    private void KeepSpawnCard(CardCollection cards)
    {
        if (graph.TryGetValue(cards.ToString(), out CardRecipe recipe))
        {
            List<CardEntry> removingCards = new();
            List<CardEntry> spawnedTargets = new();

            foreach (Cards cardType in recipe.GetConsumables())
            {
                Card consumable = cards.FindFirstByCardType(cardType);
                removingCards.Add(consumable.gameObject.GetComponent<CardEntry>());
            }

            foreach (Cards cardType in recipe.GetProducts())
            {
                paths.TryGetValue(cardType, out GameObject cardPrefab);
                GameObject cardObject = GameObject.Instantiate(cardPrefab);
                spawnedTargets.Add(cardObject.GetComponent<CardEntry>());
            }

            cards.SpawnHandler(removingCards, spawnedTargets);
        }
        else
            throw new Exception("No cards in this collection");
    }   

    public void PauseSpawnCard(CardCollection cards)
    {
        if(collectionTimePairs.ContainsKey(cards))
        {
            if (!pausingCollections.Contains(cards))
                pausingCollections.Add(cards);
        }
    }

    /**
     * Set up the dictionary, takes recipes as content.
     */
    private void Initialize()
    {
        if (graph == null)
        {
            controller = CardController.GetInstance();
            graph = new Dictionary<string, CardRecipe>();

            List<CardRecipe> recipes = controller.GetCardRecipes();

            foreach (CardRecipe recipe in recipes)
            {
                string key = recipe.GetKey();
                if (recipe.GetConsumables().Count == 0)
                    throw new Exception("You forget to update recipe in editor.");
                graph.Add(key, recipe);
                // Debug.Log("Add Recipe Entry: " + key + " " + recipe.GetTarget().ToString());
            }
        }

        if(paths == null)
        {
            paths = new Dictionary<Cards, GameObject>();

            foreach(PrefabPath path in controller.paths)
            {
                paths.Add(path.prefab, path.prefabObject);
            }
        }
    }

    public static CardFactory GetInstance()
    {
        return cardFactory;
    }
}
