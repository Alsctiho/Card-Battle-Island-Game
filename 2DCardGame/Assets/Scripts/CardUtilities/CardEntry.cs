using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CollectionActionState
{
    Merge,
    Split,
    Move,
    PutBack,
}

public class CardEntry : MonoBehaviour
{
    public CardCollection cardCollection = null;
    private CardFactory cardFactory = null;

    private CardEntry thisCard;
    private CardEntry otherCard;

    // Doubly linked list to store the card relationship.

    private void Start()
    {
        thisCard = this;
        if(cardCollection == null)
        {
            cardCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
            cardCollection.Register(this);
        }
        cardFactory = CardFactory.GetInstance();
    }

    public void DragHandler(Vector3 parentPosition)
    {
        cardFactory.PauseSpawnCard(cardCollection);
        cardCollection.UpdateCardsPosition(this, parentPosition);
    }

    public void DropHandler(Vector3 mousePosition)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(mousePosition, Card.dimensions, 0.0f, new Vector2(0.0f, 0.0f));
        CollectionActionState state;
        bool needPutBack = false;
        bool continueChecking = true;
        otherCard = null;

        foreach(RaycastHit2D hit in hits)
        {
            CardEntry temp = hit.transform.gameObject.GetComponent<CardEntry>();
            if (temp && temp.cardCollection != this.cardCollection)
            {
                continueChecking = false;
                otherCard = hit.transform.gameObject.GetComponent<CardEntry>();
                break;
            }
            else
            {
                if (needPutBack)
                    continue;

                if(temp && ExistsInPrevs(temp))
                {
                    needPutBack = true;
                    continue;
                }
            }
        }
        
        if(continueChecking)
        {
            if (needPutBack)
            {
                state = CollectionActionState.PutBack;
            }
            else if(cardCollection.Head() == this)
            {
                state = CollectionActionState.Move;
            }
            else
            {
                state = CollectionActionState.Split;
            }
        }
        else
        {
            state = CollectionActionState.Merge;
        }

        switch(state)
        {
            case CollectionActionState.Merge:
                cardFactory.ClearCollection(cardCollection);
                cardFactory.ClearCollection(otherCard.cardCollection);
                MergeCollections();
                cardFactory.RequestSpawnCard(cardCollection); // Here is the new cardcollection.
                break;

            case CollectionActionState.Move:
                MoveCollections();
                cardFactory.RequestSpawnCard(cardCollection);
                break;

            case CollectionActionState.PutBack:
                PutBackCollections();
                cardFactory.RequestSpawnCard(cardCollection);
                break;

            case CollectionActionState.Split:
                CardCollection oldCollection = cardCollection;
                cardFactory.ClearCollection(oldCollection);
                SplitCollections();
                cardFactory.RequestSpawnCard(cardCollection); // Here is the new cardcollection
                cardFactory.RequestSpawnCard(oldCollection);
                break;
        }
    }

    private bool ExistsInPrevs(CardEntry other)
    {
        return cardCollection.ExistsBeforeThisCard(other, this);
    }

    private void PutBackCollections()
    {
        Vector3 newPosition = GetPrev().transform.position + Card.cardPositionOffset;
        cardCollection.UpdateCardsPosition(this, newPosition);
    }

    private void MergeCollections()
    {
        if(thisCard.cardCollection.Head() != thisCard)
        {
            //First split.
            thisCard.SplitCollections();
        }

        // Then merge.
        CardCollection otherCollection = otherCard.cardCollection;

        //Move all cards in this collection to new collection.
        otherCollection.Register(this.cardCollection);

        // Update cards' positions.
        if (GetPrev() == null)
            throw new System.Exception("previous node is null");

        Vector3 newPosition = GetPrev().transform.position + Card.cardPositionOffset;
        cardCollection.UpdateCardsPosition(this, newPosition);
    }

    private void SplitCollections()
    {
        CardCollection newCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        newCollection.RemoveAndRegisterCards(this);
        cardCollection.UpdateCardsPosition(this, new Vector3(transform.position.x, transform.position.y, Card.cardDropZOffset));
    }

    private void MoveCollections()
    { 
        cardCollection.UpdateCardsPosition(this, new Vector3(transform.position.x, transform.position.y, Card.cardDropZOffset)); 
    }

    public CardEntry GetPrev()
    {
        return cardCollection.cardEntryList.Find(this).Previous.Value;
    }

    public CardEntry GetNext()
    {
        return cardCollection.cardEntryList.Find(this).Next.Value;
    }
}
