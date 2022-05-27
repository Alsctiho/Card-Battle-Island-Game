using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBehaviour: MonoBehaviour
{
    public abstract bool CanBeConsumedBySpawn();

    public abstract void ConsumedBySpawn();
}
