using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Cards cardType;
    [TextArea(5, 10)] [SerializeField] private string description;
    public CardBehaviour cardBehaviour;

    public static Vector2 dimensions = new (1.5f, 1.6f);
    public static Vector3 cardPositionOffset = new (0.0f, -0.4f, -0.01f);
    public static float cardHoldingZOffset = -5.0f;
    public static float cardDropZOffset = -1.0f;

    public bool faceUp = true;

    public bool CanBeConsumedBySpawn()
    {
        if (cardBehaviour == null)
            throw new System.Exception("You forget to update cardBehavior in prefab");

        return cardBehaviour.CanBeConsumedBySpawn();
    }

    public void ConsumedBySpawn()
    {
        if (cardBehaviour == null)
            throw new System.Exception("You forget to update cardBehavior in prefab");

        cardBehaviour.ConsumedBySpawn();
    }
}
