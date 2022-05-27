using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minions : Battleable
{
    public override bool CanBeConsumedBySpawn()
    {
        return false;
    }

    public override void ConsumedBySpawn()
    {
        return;
    }
}
