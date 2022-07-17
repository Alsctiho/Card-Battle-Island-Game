using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public enum Belonging
    {
        Player,
        Emeny,
        Neutral,
    }

    public Belonging belongsToPlayer = Belonging.Neutral;
    [HideInInspector] public bool isInBattle = false;

    [SerializeField] private List<CardEntry> resourceCards;
    [SerializeField] private List<CardEntry> playerCards;
    [SerializeField] private List<CardEntry> emenyCards;
    private Collider2D _collider = null;

    private void Awake()
    {
        resourceCards = new();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (isInBattle)
            return;

        if (IsBattleRequired())
        {
            isInBattle = true;
            InitBattle();
        }
    }

    private void InitBattle()
    {
        gameObject.GetComponent<Battle>().InitBattle(belongsToPlayer, playerCards, emenyCards);
    }

    public void UpdateBattleInfo(Card newcard)
    {
        gameObject.GetComponent<Battle>().UpdateBattleInfo(newcard.GetComponent<Battleable>());
    }

    public void BattleEnd()
    {
        isInBattle = false;
    }

    private void Initialize()
    {
        RaycastHit2D[] hits = new RaycastHit2D[128];
        int numberOfHits = _collider.Cast(Vector2.zero, hits);

        for(int i = 0; i < numberOfHits; ++i)
        {
            CardEntry temp = hits[i].transform.gameObject.GetComponent<CardEntry>();
            Register(temp);
        }
    }

    public void RegisterAllCards(List<CardEntry> newcards)
    {
        foreach (CardEntry cardEntry in newcards)
            Register(cardEntry);
    }

    public void Register(CardEntry newCardEntry)
    {
        if (newCardEntry.island == this)
            return;

        if(newCardEntry.island)
            newCardEntry.island.Remove(newCardEntry);

        Card newcard = newCardEntry.gameObject.GetComponent<Card>();
        Belonging belonging = Belonging.Neutral;
        if (newcard.IsResource())
        {
            resourceCards.Add(newCardEntry);
        }
        else
        {
            if (newcard.IsPlayer())
            {
                playerCards.Add(newCardEntry);
                belonging = Belonging.Player;
            }
            else
            {
                emenyCards.Add(newCardEntry);
                belonging = Belonging.Emeny;
            }

            if(isInBattle)
            {
                UpdateBattleInfo(newcard);
            }
        }
        newCardEntry.island = this;

        if (belongsToPlayer == Belonging.Neutral)
            belongsToPlayer = belonging;
    }

    public void Remove(Battleable oldBttleable)
    {
        Remove(oldBttleable.GetComponent<CardEntry>());
    }

    public void Remove(CardEntry oldCardEntry)
    {
        Card oldcard = oldCardEntry.gameObject.GetComponent<Card>();
        if (oldcard.IsResource())
            resourceCards.Remove(oldCardEntry);
        else if (oldcard.IsPlayer())
            playerCards.Remove(oldCardEntry);
        else
            emenyCards.Remove(oldCardEntry);
    }

    public bool IsBattleRequired()
    {
        return playerCards.Count != 0 && emenyCards.Count != 0;
    }
}
