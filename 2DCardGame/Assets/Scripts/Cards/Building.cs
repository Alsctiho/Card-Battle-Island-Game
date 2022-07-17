using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Battleable
{
    public override bool ConsumedBySpawn()
    {
        return false;
    }
}
