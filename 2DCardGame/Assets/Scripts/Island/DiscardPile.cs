
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public static GameObject discardPileCanvas;
    private static List<Card> discards = new();

    private void Awake()
    {
        discardPileCanvas = GameObject.Find("Discard Pile");
    }

    public static void Register(Card card)
    {
        card.transform.SetParent(discardPileCanvas.transform, false);
        card.transform.position = Vector3.zero;
        card.gameObject.SetActive(false);
        discards.Add(card);
    }

    public static void Register(Battleable battleable)
    {
        Register(battleable.GetComponent<Card>());
    }

    public static void Register(CardBehaviour behaviour)
    {
        Register(behaviour.GetComponent<Card>());
    }
}
