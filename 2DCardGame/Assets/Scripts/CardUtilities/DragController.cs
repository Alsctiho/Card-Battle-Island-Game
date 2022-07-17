using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    static private DragController controller = null;
    private bool isDragActive = false;
    
    private Vector3 offset;
    private Vector3 worldPosition; // mouse position in world space.
    private Draggable lastDraggable = null;

    private void Awake()
    {
        if (controller && controller != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            controller = this;
        }
    }

    private void Update()
    {
        if(isDragActive && Input.GetMouseButtonUp(0))
        {
            Drop();
            return;
        }

        if (!Input.GetMouseButton(0))
            return;

        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //worldPosition = Input.mousePosition;

        if (isDragActive)
            Drag();
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if(hit.collider)
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if(draggable)
                {
                    lastDraggable = draggable;
                    InitDrag();
                }
            }
        }
    }

    private void InitDrag()
    {
        isDragActive = true;
        offset = lastDraggable.transform.position - worldPosition;
        lastDraggable.InitDrag();
    }

    private void Drag()
    {
        Vector3 newPosition = worldPosition + offset;
        lastDraggable.Drag(newPosition);
    }

    private void Drop()
    {
        isDragActive = false;
        lastDraggable.Drop(worldPosition);
    }
}
