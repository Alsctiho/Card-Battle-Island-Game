using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public enum Result
    {
        OpponentDead,
        OpponentAlive,
    }

    [SerializeField] private Panel playerPanel;
    [SerializeField] private Panel emenyPanel;

    private bool isInBattle = false;
    private List<Battleable> attackSequence = new();
    private List<Battleable> removingCards = new();
    private List<Battleable> resettingCards = new();

    // Update is called once per frame
    void Update()
    {
        if (isInBattle == false)
            return;

        removingCards.Clear();
        resettingCards.Clear();

        float timePass = Time.deltaTime;

        foreach(Battleable battleable in attackSequence)
        {
            if (removingCards.Contains(battleable))
            {
                Debug.Log("continue " + battleable);
                continue;
            }

            battleable.timeLeft -= timePass;

            if(battleable.timeLeft <= 0.0f)
            {
                Result result = battleable.Attack(OpponentCards(battleable), out List<Battleable> deadCards);
                
                switch (result)
                {
                    case Result.OpponentAlive:
                        battleable.timeLeft = battleable.attackPeriod;
                        break;

                    case Result.OpponentDead:
                        Debug.Log("Someone dead");
                        foreach(Battleable deadCard in deadCards)
                        {
                            removingCards.Add(deadCard);
                        }

                        break;
                }
            }
        }

        foreach(Battleable deadCard in removingCards)
        {
            Debug.Log("remove" + deadCard);
            attackSequence.Remove(deadCard);
            if (deadCard.isPlayer)
            {
                playerPanel.HandleDeath(deadCard);
                transform.GetComponent<Island>().Remove(deadCard);
            }
            else
            {
                emenyPanel.HandleDeath(deadCard);
                transform.GetComponent<Island>().Remove(deadCard);
            }
        }

        if(emenyPanel.Count == 0)
        {
            foreach(Battleable battleable in playerPanel.GetBattleables())
            {
                resettingCards.Add(battleable);
            }

            foreach(Battleable battleable in resettingCards)
            {
                playerPanel.HandleAlive(battleable);
            }

            BattleEnd();
        }
        else if(playerPanel.Count == 0)
        {
            foreach (Battleable battleable in emenyPanel.GetBattleables())
            {
                resettingCards.Add(battleable);
            }

            foreach (Battleable battleable in resettingCards)
            {
                emenyPanel.HandleAlive(battleable);
            }

            BattleEnd();
        }
    }

    private List<Battleable> OpponentCards(Battleable battleable)
    {
        return battleable.isPlayer ? emenyPanel.GetBattleables() : playerPanel.GetBattleables();
    }

    public void InitBattle(Island.Belonging belonging, List<CardEntry> playerCards, List<CardEntry> emenyCards)
    {
        Debug.Log("InitBattle");
        isInBattle = true;

        // Handle Display
        foreach (CardEntry card in playerCards)
        {
            playerPanel.Register(card.GetComponent<Battleable>());
        }

        foreach (CardEntry card in emenyCards)
        {
            emenyPanel.Register(card.GetComponent<Battleable>());
        }

        // Battle Sequence
        if(belonging == Island.Belonging.Player)
        {
            foreach(CardEntry card in emenyCards)
            {
                attackSequence.Add(card.GetComponent<Battleable>());
            }

            foreach (CardEntry card in playerCards)
            {
                attackSequence.Add(card.GetComponent<Battleable>());
            }
        }
        else
        {
            foreach (CardEntry card in playerCards)
            {
                attackSequence.Add(card.GetComponent<Battleable>());
            }

            foreach (CardEntry card in emenyCards)
            {
                attackSequence.Add(card.GetComponent<Battleable>());
            }
        }
    }

    public void UpdateBattleInfo(Battleable newBattleable)
    {
        if(newBattleable.isPlayer)
        {
            playerPanel.Register(newBattleable);
        }
        else
        {
            emenyPanel.Register(newBattleable);
        }

        attackSequence.Add(newBattleable);
    }

    private void BattleEnd()
    {
        Debug.Log("battle end");
        isInBattle = false;
        attackSequence.Clear();
        playerPanel.Clear();
        emenyPanel.Clear();
        gameObject.GetComponent<Island>().BattleEnd();
    }
}
