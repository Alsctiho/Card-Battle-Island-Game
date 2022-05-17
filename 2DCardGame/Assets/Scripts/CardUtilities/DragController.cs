using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    static private DragController controller = null;
    private DragAndDrop dragger = null;

    private void Awake()
    {
        if(controller && controller != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            controller = this;
        }
    }

    public void Register(DragAndDrop dragger)
    {
        //Debug.Log("resigter");
        this.dragger = dragger;
    }

    public void Remove()
    {
        //Debug.Log("remove");
        dragger = null;
    }

    public bool IsDraggable()
    {
        return dragger == null;
    }

    public bool IsThisDraggable(DragAndDrop other)
    {
        return dragger == other;
    }

    static public DragController Instantiate()
    {
        return controller;
    }
}
