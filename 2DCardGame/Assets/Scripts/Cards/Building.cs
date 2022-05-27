using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Battleable
{
    public override bool CanBeConsumedBySpawn()
    {
        Debug.Log("consumed this building");
        return false;
    }

    public override void ConsumedBySpawn()
    {
        return;
    }
}
