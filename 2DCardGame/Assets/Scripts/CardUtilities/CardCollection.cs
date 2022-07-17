using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : ScriptableObject
{
    [HideInInspector] public LinkedList<CardEntry> cardEntryList;
    [HideInInspector] public Island island = null;

    public void Awake()
    {
        cardEntryList = new();
    }

    public void TimerHandler()
    {

    }

    public void SpawnHandler(List<CardEntry> removedCards, List<CardEntry> newcards)
    {
        Vector3 headPosition = Head().transform.position;
        Island island = Head().island;

        foreach(CardEntry removedCard in removedCards)
        {
            if(removedCard.gameObject.GetComponent<Card>().ConsumedBySpawn())
            {
                Remove(removedCard);
                island.Remove(removedCard);
            }
        }

        foreach (CardEntry newcard in newcards)
        {
            Register(newcard);
            island.Register(newcard);
        }

        UpdateCardsPositions(Head(), headPosition);
        UpdateCardsHierarchies(Head());
    }

    public void Register(CardEntry newcard)
    {
        cardEntryList.AddLast(newcard);
        newcard.cardCollection = this;
    }

    public void Remove(CardEntry oldcard)
    {
        cardEntryList.Remove(oldcard);
        oldcard.cardCollection = null;
    }

    public void Register(CardCollection collection)
    {
        if (this == collection)
            return;

        foreach(CardEntry cardEntry in collection.cardEntryList)
        {
            Register(cardEntry);
        }
    }

    // Remove all cards start from newcard.
    public void RemoveAndRegisterCards(CardEntry removedcard)
    {
        LinkedList<CardEntry> otherCardList = removedcard.cardCollection.cardEntryList;
        LinkedListNode <CardEntry> workingNode = otherCardList.Find(removedcard);

        while(workingNode != null)
        {
            LinkedListNode<CardEntry> temp = workingNode.Next;
            CardEntry cardEntry = workingNode.Value;
            otherCardList.Remove(cardEntry);
            this.Register(cardEntry);
            workingNode = temp;
        }
    }

    public bool ExistsBeforeThisCard(CardEntry target, CardEntry threshold)
    {
        foreach(CardEntry cardEntry in cardEntryList)
        {
            if (cardEntry == threshold)
                return false;

            if (target == cardEntry)
                return true;
        }
        throw new System.Exception("No found");
    }

    public void UpdateCardsPositions(CardEntry target, Vector3 targetPosition)
    {
        LinkedListNode<CardEntry> workingNode = cardEntryList.Find(target);
        Vector3 workingPosition = targetPosition;

        while(workingNode != null)
        {
            workingNode.Value.transform.position = workingPosition;
            workingNode = workingNode.Next;
            workingPosition += Card.cardPositionOffset;
        }
    }

    public void UpdateCardsHierarchies(CardEntry target)
    {
        List<CardEntry> cardEntries = GetAllCardsFrom(target);
        foreach (CardEntry cardEntry in cardEntries)
        {
            cardEntry.transform.SetAsLastSibling();
        }
    }

    public Card FindFirstByCardType(CardType cardType)
    {
        foreach(CardEntry cardEntry in cardEntryList)
        {
            Card workingCard = cardEntry.gameObject.GetComponent<Card>();
            if (workingCard.cardType == cardType)
            {
                return workingCard;
            }
        }
        throw new System.Exception("Card Type cannot find");
    }

    public List<CardEntry> GetAllCardsFrom(CardEntry cardEntry)
    {
        List<CardEntry> cards = new();
        bool NeedAdd = false;
        foreach(CardEntry card in cardEntryList)
        {
            if(card == cardEntry)
                NeedAdd = true;

            if (NeedAdd)
                cards.Add(card);
        }

        return cards;
    }

    public CardEntry Head()
    {
        return cardEntryList.First.Value;
    }

    public CardEntry Last()
    {
        return cardEntryList.Last.Value;
    }

    public override string ToString()
    {
        List<string> strings = new();
        foreach(CardEntry entry in cardEntryList)
        {
            strings.Add(entry.gameObject.GetComponent<Card>().cardType.ToString());
        }
        strings.Sort();
        string cardNames = "";
        for (int i = 0; i < strings.Count - 1; ++i)
            cardNames += strings[i] + "+";
        cardNames += strings[^1];
        return cardNames;
    }
}
