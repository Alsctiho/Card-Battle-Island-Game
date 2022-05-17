using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    public bool isDragging = false;
    private bool isDragable = false;

    public Vector3 offset;
    private float yOffset = 0.4f;

    private DragController controller = null;
    private new Camera camera;
    private CardEntry cardEntry = null;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        controller = DragController.Instantiate();
        camera = Camera.main;
        cardEntry = gameObject.GetComponent<CardEntry>();
    }

    void OnMouseEnter()
    {
        //Debug.Log("mouse enter");
        if(controller.IsDraggable())
        {
            controller.Register(this);
            isDragable = true;
        }
    }

    private void OnMouseExit()
    {
        if (isDragging)
            return;

        if (controller.IsThisDraggable(this))
        {
            controller.Remove();
            isDragable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragable == false)
            return;

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Drop();
            return;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Drag();
        } 
        else if(Input.GetMouseButton(0))
        {
            isDragging = true;
            // init drag
            offset = transform.position - mousePosition();
            Drag();
        }
    }

    private void Drag()
    {
        Vector3 parentPosition = mousePosition() + offset;
        parentPosition.z = -1.0f;
        transform.position = parentPosition;

        CardEntry workingCard = cardEntry.GetNext();
        for(;workingCard != null; workingCard = workingCard.GetNext())
        {
            parentPosition += new Vector3(0, -yOffset, -0.01f);
            workingCard.transform.position = parentPosition;
        }
    }

    private void Drop()
    {
        //Debug.Log("drop");
        cardEntry.DropHandler();
    }

    private Vector3 mousePosition()
    {
        Vector3 mousePosition =  camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        return mousePosition;
    }
}
