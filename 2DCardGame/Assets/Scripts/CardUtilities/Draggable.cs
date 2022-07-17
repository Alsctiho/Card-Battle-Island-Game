using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    CardEntry cardEntry = null;
    public bool isDragging = false;

    private void Start()
    {
        cardEntry = gameObject.GetComponent<CardEntry>();
    }

    public void InitDrag()
    {
        cardEntry.InitDragHandler();
        AudioManager.Play("cardpickup");
    }

    public void Drag(Vector3 parentPosition)
    {
        isDragging = true;

        parentPosition.z = Card.cardHoldingZOffset;
        cardEntry.DragHandler(parentPosition);
    }

    public void Drop(Vector3 mousePosition)
    {
        //Debug.Log("drop");
        isDragging = false;
        cardEntry.DropHandler(mousePosition);

        AudioManager.Play("cardplace");
    }
}
