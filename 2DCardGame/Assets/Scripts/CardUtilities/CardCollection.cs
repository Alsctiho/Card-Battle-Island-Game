using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : ScriptableObject
{
    private List<string> cardstrings;

    private CardEntry head = null;
    private CardEntry tail = null;
    private int Size
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
    public void Initialize(CardEntry firstcard)
    {
        SetHeadCard(firstcard);
        SetTailCard(firstcard);

        cardstrings.Add(firstcard.ToString());
        firstcard.cardCollection = this;
    }

    public void Register(CardEntry newcard)
    {
        cardstrings.Add(newcard.ToString());

        //Debug.Log("update tail");
        tail.next = newcard;
        newcard.prev = tail;
        SetTailCard(newcard);
        newcard.cardCollection = this;
    }

    public void Register(CardCollection collection)
    {
        if (this == collection)
            return;

        for (CardEntry workingCard = collection.GetHeadCard(); workingCard != null; workingCard = workingCard.GetNext())
        {
            Register(workingCard);
        }
    }


    public void Remove(CardEntry removedcard)
    {
        if (tail == removedcard)
            tail = removedcard.prev;

        if(removedcard.prev)
            removedcard.prev.next = removedcard.next;

        if(removedcard.next)
            removedcard.next.prev = removedcard.prev;

        cardstrings.Remove(removedcard.ToString());

        removedcard.next = null;
        removedcard.prev = null;
        removedcard.cardCollection = null;
    }

    public void SetHeadCard(CardEntry header)
    {
        this.head = header;
    }

    public CardEntry GetHeadCard()
    {
        return head;
    }

    public void SetTailCard(CardEntry tail)
    {
        this.tail = tail;
    }

    public CardEntry GetTailCard()
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
