using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minions : Battleable
{
    public override bool ConsumedBySpawn()
    {
        return false;
    }
}
