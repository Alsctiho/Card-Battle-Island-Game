using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : ScriptableObject
{
    private List<string> cardstrings;

    private Card head = null;
    private Card tail = null;
    private int size
    {
        get
        {
            return cardstrings.Count;
        }
    }

    public void Awake()
    {
        cardstrings = new List<string>();
    }

    /**
     * Insert the first card, and set up the Head and the Tail of this Collection.
     */
    public void Initialize(Card firstcard)
    {
        SetHeadCard(firstcard);
        SetTailCard(firstcard);

        cardstrings.Add(firstcard.ToString());
        firstcard.cardCollection = this;
    }

    public void Register(Card newcard)
    {
        cardstrings.Add(newcard.ToString());

        Debug.Log("update tail");
        tail.next = newcard;
        newcard.prev = tail;
        SetTailCard(newcard);
        newcard.cardCollection = this;
    }

    public void Register(CardCollection collection)
    {
        if (this == collection)
            return;

        for (Card workingCard = collection.GetHeadCard(); workingCard != null; workingCard = workingCard.GetNext())
        {
            Register(workingCard);
        }
    }


    public void Remove(Card removedcard)
    {
        if (tail == removedcard)
            tail = removedcard.prev;

        removedcard.prev.next = removedcard.next;
        removedcard.next.prev = removedcard.prev;

        removedcard.next = null;
        removedcard.prev = null;
        removedcard.cardCollection = null;
    }

    public void SetHeadCard(Card header)
    {
        this.head = header;
    }

    public Card GetHeadCard()
    {
        return head;
    }

    public void SetTailCard(Card tail)
    {
        this.tail = tail;
    }

    public Card GetTailCard()
    {
        return tail;
    }

    public override string ToString()
    {
        List<string> temp = new List<string>(cardstrings);
        temp.Sort();
        string cardNames = "";
        for (int i = 0; i < temp.Count - 1; ++i)
            cardNames += temp[i] + "+";

        cardNames += temp[temp.Count - 1];
        return cardNames;
    }
}
