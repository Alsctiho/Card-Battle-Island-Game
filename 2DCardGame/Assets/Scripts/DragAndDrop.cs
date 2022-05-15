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
    private Card card = null;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        controller = DragController.Instantiate();
        camera = Camera.main;
        card = gameObject.GetComponent<Card>();
    }

    void OnMouseEnter()
    {
        // Debug.Log("enter");
        if(controller.IsDraggable())
        {
            controller.Register();
            isDragable = true;
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
            drop();
            return;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            drag();
        } 
        else if(Input.GetMouseButton(0))
        {
            isDragging = true;
            // init drag
            offset = transform.position - mousePosition();
            drag();
        }
    }

    private void drag()
    {
        Vector3 parentPosition = mousePosition() + offset;
        transform.position = parentPosition;

        Card workingCard = card.GetNext();
        for(;workingCard != null; workingCard = workingCard.GetNext())
        {
            parentPosition += new Vector3(0, -yOffset, -0.01f);
            workingCard.transform.position = parentPosition;
        }
    }

    private void drop()
    {
        //Debug.Log("drop");
        isDragable = false;
        controller.Remove();
        card.DropHandler();
    }

    private Vector3 mousePosition()
    {
        Vector3 mousePosition =  camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        return mousePosition;
    }
}
