using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBehaviour: MonoBehaviour
{
    private void Start()
    {
        Debug.Log("card behavior start");
    }

    protected void UpdateHealthDisplay(int newHealth)
    {
        transform.gameObject.GetComponent<CardDisplay>().UpdateHealth(newHealth);
    }

    protected void UpdateAttackDisplay(int newAttack)
    {
        transform.gameObject.GetComponent<CardDisplay>().UpdateAttack(newAttack);
    }

    public abstract void Initialize(ObjectStatus status);

    public abstract bool ConsumedBySpawn();
}
