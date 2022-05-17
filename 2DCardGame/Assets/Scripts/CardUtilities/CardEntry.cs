using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEntry : MonoBehaviour
{
    public CardCollection cardCollection = null;
    private DragAndDrop dragger = null;

    // other card keep track of the 
    private CardEntry otherCard = null;
    private bool needSplit = false;

    // Doubly linked list to store the card relationship.
    public CardEntry prev = null;
    public CardEntry next = null;

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
        otherCard = other.gameObject.GetComponent<CardEntry>();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (dragger.isDragging == false)
            return;

        if (other.gameObject.GetComponent<CardEntry>() == otherCard)
        {
            otherCard = null;
            needSplit = true;
        }
    }

    public void DropHandler()
    {
        if (needSplit && otherCard)
            SplitAndMergeCollections();
        else if (needSplit)
            SplitCollections();
        else if (otherCard)
            MergeCollections();
        else
            MoveCollections();
    }

    private void SplitAndMergeCollections()
    {
        for (CardEntry workingCard = this; workingCard; workingCard = workingCard.next)
        {
            cardCollection.Remove(workingCard);
            otherCard.cardCollection.Register(workingCard);
        }

        Vector3 newPosition = prev.transform.position + new Vector3(0.0f, -0.4f, -0.01f);
        UpdateCollectionPosition(newPosition);

        needSplit = false;
    }

    private void MergeCollections()
    {
        CardCollection otherCollection = otherCard.cardCollection;

        //Move all cards in this collection to new collection.
        otherCollection.Register(this.cardCollection);

        // Update cards' positions.
        if (prev == null)
            throw new System.Exception("previous node is null");

        Vector3 newPosition = prev.transform.position + new Vector3(0.0f, -0.4f, -0.01f);
        UpdateCollectionPosition(newPosition);
    }

    private void SplitCollections()
    {
        CardCollection newCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        cardCollection.Remove(this);
        newCollection.Initialize(this);

        for (CardEntry workingCard = this.next; workingCard; workingCard = workingCard.next)
        {
            cardCollection.Remove(workingCard);
            newCollection.Register(workingCard);
        }

        UpdateCollectionPosition(new Vector3(transform.position.x, transform.position.y, -0.1f));

        needSplit = false;
    }

    private void MoveCollections()
    {
        //Debug.Log("Moving");
        UpdateCollectionPosition(new Vector3(transform.position.x, transform.position.y, -0.1f));
    }

    private void UpdateCollectionPosition(Vector3 parentPosition)
    {
        for (CardEntry workingCard = this; workingCard; workingCard = workingCard.next)
        {
            workingCard.transform.position = parentPosition;
            parentPosition += new Vector3(0.0f, -0.4f, -0.01f);
        }
    }

    public void SetPrev(CardEntry newprev)
    {
        prev = newprev;
    }
    public CardEntry GetPrev()
    {
        return prev;
    }

    public void SetNext(CardEntry newnext)
    {
        next = newnext;
    }

    public CardEntry GetNext()
    {
        return next;
    }
}
