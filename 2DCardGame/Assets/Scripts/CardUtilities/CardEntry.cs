using System;
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
    [HideInInspector] public CardCollection cardCollection = null;
    [HideInInspector] public Island island = null;

    private Collider2D _collider;
    private CardFactory cardFactory = null;
    private CardEntry otherCard;
    private Vector3 originalPosition;

    // Doubly linked list to store the card relationship.

    private void Start()
    {
        if(cardCollection == null)
        {
            cardCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
            cardCollection.Register(this);
        }
        _collider = gameObject.GetComponent<Collider2D>();
        cardFactory = CardFactory.GetInstance();
    }

    public void InitDragHandler()
    {
        originalPosition = transform.position;
        cardCollection.UpdateCardsHierarchies(this);
    }

    public void DragHandler(Vector3 parentPosition)
    {
        cardFactory.PauseSpawnCard(cardCollection);
        cardCollection.UpdateCardsPositions(this, parentPosition);
    }

    public void DropHandler(Vector3 mousePosition)
    {
        // Collection Detection
        RaycastHit2D[] cardHits = Physics2D.BoxCastAll(mousePosition, Card.dimensions, 0.0f, new Vector2(0.0f, 0.0f));
        CollectionActionState state;
        bool needPutBack = false;
        bool needMerge = false;
        otherCard = null;

        foreach(RaycastHit2D hit in cardHits)
        {
            CardEntry cardEntry = hit.transform.gameObject.GetComponent<CardEntry>();

            if (cardEntry)
            {
                if (cardEntry.cardCollection != this.cardCollection)
                {
                    needMerge = true;
                    otherCard = hit.transform.gameObject.GetComponent<CardEntry>();
                    break;
                }
                else
                {
                    if (needPutBack)
                        continue;

                    if (ExistsInPrevs(cardEntry))
                    {
                        needPutBack = true;
                        continue;
                    }
                }
            }
        }

        if (needPutBack)
        {
            state = CollectionActionState.PutBack;
        }
        else if (needMerge)
        {
            state = CollectionActionState.Merge;
        }
        else if (cardCollection.Head() == this)
        {
            state = CollectionActionState.Move;
        }
        else
        {
            state = CollectionActionState.Split;
        }

        // Island Detection
        bool needPutBack1 = true;
        RaycastHit2D[] objectHits = new RaycastHit2D[16];
        int numberOfIslands = _collider.Cast(Vector2.zero, objectHits);
        for(int i = 0; i < numberOfIslands; ++i)
        {
            Island otherIsland = objectHits[i].transform.gameObject.GetComponent<Island>();
            if (otherIsland)
            {
                needPutBack1 = false;
                List<CardEntry> nextCards = cardCollection.GetAllCardsFrom(this);
                otherIsland.RegisterAllCards(nextCards);
            }
        }

        if(needPutBack1)
            state = CollectionActionState.PutBack;

        switch (state)
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
        cardCollection.UpdateCardsPositions(this, originalPosition);
    }

    private void MergeCollections()
    {
        if(cardCollection.Head() != this)
        {
            //First split.
            SplitCollections();
        }

        // Then merge.
        CardCollection otherCollection = otherCard.cardCollection;

        //Move all cards in this collection to new collection.
        otherCollection.Register(this.cardCollection);

        // Update cards' positions.
        if (GetPrev() == null)
            throw new System.Exception("previous node is null");

        Vector3 newPosition = GetPrev().transform.position + Card.cardPositionOffset;
        cardCollection.UpdateCardsPositions(this, newPosition);
    }

    private void SplitCollections()
    {
        CardCollection newCollection = ScriptableObject.CreateInstance("CardCollection") as CardCollection;
        newCollection.RemoveAndRegisterCards(this);
        cardCollection.UpdateCardsPositions(this, new Vector3(transform.position.x, transform.position.y, Card.cardDropZOffset));
    }

    private void MoveCollections()
    { 
        cardCollection.UpdateCardsPositions(this, new Vector3(transform.position.x, transform.position.y, Card.cardDropZOffset)); 
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
