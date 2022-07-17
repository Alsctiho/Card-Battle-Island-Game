using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public int Count
    {
        get { return battleables.Count; }
    }

    private GameObject cardCanvas;
    [SerializeField] private List<Battleable> battleables = new();
    private Dictionary<Battleable, Vector3> originalPositions = new();

    private void Awake()
    {
        cardCanvas = GameObject.Find("Card Canvas");
    }

    public void Register(Battleable newcard)
    {
        Debug.Log("register " + newcard);
        battleables.Add(newcard);
        originalPositions.Add(newcard, newcard.transform.position);
        newcard.transform.SetParent(gameObject.transform, true);
    }

    public void Clear()
    {
        battleables.Clear();
        originalPositions.Clear();
    }

    /*
     * Handle if the card is alive.
     */
    public void HandleAlive(Battleable oldcard)
    {
        Debug.Log("set back old card" + oldcard);
        oldcard.transform.SetParent(cardCanvas.transform, false);
        oldcard.transform.position = Remove(oldcard);
    }

    /*
     * Handle if the card is dead. 
     */
    public void HandleDeath(Battleable oldcard)
    {
        Remove(oldcard);
        DiscardPile.Register(oldcard);
    }

    public List<Battleable> GetBattleables()
    {
        return battleables;
    }

    private Vector3 Remove(Battleable oldCard)
    {
        if (originalPositions.TryGetValue(oldCard, out Vector3 position))
        {
            battleables.Remove(oldCard);
            originalPositions.Remove(oldCard);
            return position;
        }
        else
        {
            throw new System.Exception("No card found" + oldCard);
        }
    }
}
