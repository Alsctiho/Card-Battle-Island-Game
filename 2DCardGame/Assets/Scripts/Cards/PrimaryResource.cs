using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryResource : Resource
{
    int health;

    public override void Initialize(ObjectStatus status)
    {
        if (status.health == 0)
            throw new System.Exception("Not Update status in editor");

        health = status.health;
        UpdateHealthDisplay(health);
    }

    public override bool ConsumedBySpawn()
    {
        health--;
        if(health <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }
        else
        {
            UpdateHealthDisplay(health);
            return false;
        }
    }
}
