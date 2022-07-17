using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryResource : Resource
{
    public override void Initialize(ObjectStatus status)
    {
        throw new System.Exception("Should not initilize secondary resource");
    }

    public override bool ConsumedBySpawn()
    {
        Debug.Log("Consumed resource");
        Destroy(this.gameObject);
        return true;
    }
}
