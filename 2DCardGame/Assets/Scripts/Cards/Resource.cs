using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource: CardBehaviour
{
    public override bool CanBeConsumedBySpawn()
    {
        return true;
    }

    public override void ConsumedBySpawn()
    {
        Debug.Log("Consumed resource");
        Destroy(this.gameObject);
    }
}
