using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardRecipe
{
    [SerializeField] public List<Cards> workers;
    [SerializeField] public List<Cards> consumables;
    [SerializeField] public List<Cards> products;
    [SerializeField] public float timeRequired;

    private string sourceStr = null;


    public List<Cards> GetConsumables()
    {
        return consumables;
    }

    public List<Cards> GetProducts()
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
            List<Cards> sources = new List<Cards>(consumables);
            foreach (Cards worker in workers)
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
