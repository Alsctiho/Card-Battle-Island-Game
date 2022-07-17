using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardType cardType;
    [TextArea(5, 10)] public string description;
    public CardBehaviour cardBehaviour;

    [HideInInspector] public static Vector2 dimensions = new (1.300f, 1.900f);
    [HideInInspector] public static Vector3 cardPositionOffset = new (0.0f, -0.40f, -0.01f);
    [HideInInspector] public static float cardHoldingZOffset = -5.0f;
    [HideInInspector] public static float cardDropZOffset = -1.0f;
    

    public bool faceUp = true;

    public bool IsPlayer()
    {
        Battleable battleable = gameObject.GetComponent<Battleable>();
        if (battleable)
            return battleable.isPlayer;
        else
            throw new System.Exception("Not battleable");
    }

    public bool IsResource()
    {
        return gameObject.GetComponent<Resource>();
    }

    public bool ConsumedBySpawn()
    {
        if (cardBehaviour == null)
            throw new System.Exception("You forget to update cardBehavior in editor");

        return cardBehaviour.ConsumedBySpawn();
    }
}
