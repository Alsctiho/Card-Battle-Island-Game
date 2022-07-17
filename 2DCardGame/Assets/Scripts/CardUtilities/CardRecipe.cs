using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardRecipe
{
    public List<CardType> workers;
    public List<CardType> consumables;
    public List<CardType> products;
    public float timeRequired;

    private string sourceStr = null;


    public List<CardType> GetConsumables()
    {
        return consumables;
    }

    public List<CardType> GetWorker()
    {
        return workers;
    }

    public List<CardType> GetProducts()
    {
        return products;
    }

    public float GetTime()
    {
        return timeRequired;
    }

    public string GetKey()
    {
        if(sourceStr == null)
        {
            List<CardType> sources = new List<CardType>(consumables);
            foreach (CardType worker in workers)
            {
                sources.Add(worker);
            }

            sources.Sort();
            string result = "";
            for (int i = 0; i < sources.Count - 1; ++i)
            {
                result += sources[i].ToString() + "+";
            }
            result += sources[sources.Count - 1].ToString();

            sourceStr = result;
        }

        return sourceStr;
    }
}
