using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Battleable: CardBehaviour
{
    public bool isPlayer;

    public int health;
    public int attack;

    public float attackPeriod;
    public float timeLeft;

    private void Start()
    {
        Debug.Log("battleable start");
    }

    public override void Initialize(ObjectStatus status)
    {
        health = status.health;
        attack = status.attack;
        attackPeriod = status.attackPeriod;
        UpdateAttackDisplay(attack);
        UpdateHealthDisplay(health); 
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthDisplay(health);
        return IsDead();
    }

    public abstract Battle.Result Attack(List<Battleable> opponentCards, out List<Battleable> deadCards);
}
