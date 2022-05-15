using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] public string cardName;
    [SerializeField] public Cards cardType;
    [TextArea(5, 10)] [SerializeField] public string description;

    public CardCollection cardCollection = null;
    private DragAndDrop dragger = null;
    private Card otherCard = null;
    private bool needSplit = false;

    // Doubly linked list to store the card relationship.
    public Card prev = null;
    public Card next = null;

    private void Start()
    {
        dragger = this.gameObject.GetComponent<DragAndDrop>();
        cardCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        cardCollection.Initialize(this);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (dragger.isDragging == false)
            return;
        //this.cardCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        //cardCollection.Register(this);
        Debug.Log("Enter: " + other.gameObject.GetComponent<Card>().cardType.ToString());
        otherCard = other.gameObject.GetComponent<Card>();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (dragger.isDragging == false)
            return;

        if (other.gameObject.GetComponent<Card>() == prev)
        {
            otherCard = null;
            needSplit = true;
        }
    }

    public void DropHandler()
    {
        if (needSplit)
            SplitCollections();
        else if (otherCard)
            MergeCollections();
    }

    private void MergeCollections()
    {
        CardCollection otherCollection = otherCard.cardCollection;

        //Move all cards in this collection to new collection.
        try
        {
            otherCollection.Register(this.cardCollection);
        } catch (System.NullReferenceException e)
        {
            Debug.Log(otherCard.cardType.ToString());
            throw e;
        }
        // Update cards' positions.
        if (prev == null)
            throw new System.Exception("previous node is null");

        Vector3 prevPosition = prev.transform.position;
        for (Card workingCard = this; workingCard; workingCard = workingCard.next)
        {
            prevPosition += new Vector3(0, -0.4f, -0.01f);
            workingCard.transform.position = prevPosition;
        }
    }

    private void SplitCollections()
    {
        CardCollection newCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        newCollection.Initialize(this);
        cardCollection.Remove(this);

        for(Card workingCard = this.next; workingCard; workingCard = workingCard.next)
        {
            cardCollection.Remove(workingCard);
            newCollection.Register(workingCard);
        }

        needSplit = false;
    }

    public void SetPrev(Card newprev)
    {
        prev = newprev;
    }
    public Card GetPrev()
    {
        return prev;
    }

    public void SetNext(Card newnext)
    {
        next = newnext;
    }

    public Card GetNext()
    {
        return next;
    }

    public override string ToString()
    {
        return cardType.ToString();
    }
}
