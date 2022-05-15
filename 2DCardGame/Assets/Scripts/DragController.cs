using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    static private DragController controller = null;
    private bool isOneDragable = false;

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

    public void Register()
    {
        //Debug.Log("resigter");
        isOneDragable = true;
    }

    public void Remove()
    {
        //Debug.Log("remove");
        isOneDragable = false;
    }

    public bool IsDraggable()
    {
        return isOneDragable == false;
    }

    static public DragController Instantiate()
    {
        return controller;
    }
}
