using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardRecipes
{
    [SerializeField] private List<Cards> sources;
    [SerializeField] private Cards target;

    public List<Cards> GetSources()
    {
        return sources;
    }

    public Cards GetTarget()
    {
        return target;
    }
}
