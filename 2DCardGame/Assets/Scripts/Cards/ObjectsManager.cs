using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public List<ObjectStatus> objectStatuses;

    private static ObjectsManager objectsManager = null;

    private void Awake()
    {
        if (objectsManager && objectsManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            objectsManager = this;
        }

        Initialize();
    }

    private void Initialize()
    {
        foreach(ObjectStatus status in objectStatuses)
        {
            Debug.Log("Initialize object");
            CardBehaviour card = status.cardType.GetComponent<CardBehaviour>();

            if (card == null)
                throw new System.Exception("Initilize failed");

            card.Initialize(status);
        }
    }
}
