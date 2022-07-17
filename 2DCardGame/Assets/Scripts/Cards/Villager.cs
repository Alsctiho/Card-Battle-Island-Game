using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Minions
{
    public override Battle.Result Attack(List<Battleable> opponentCards, out List<Battleable> deadCards)
    {
        Debug.Log("Attack");
        int target = Random.Range(0, opponentCards.Count);
        Battle.Result result;

        opponentCards[target].TakeDamage(this.attack);
        if (opponentCards[target].IsDead())
        {
            result = Battle.Result.OpponentDead;
            deadCards = new();
            deadCards.Add(opponentCards[target]);
        }
        else
        {
            result = Battle.Result.OpponentAlive;
            deadCards = null;
        }

        return result;
    }
}
