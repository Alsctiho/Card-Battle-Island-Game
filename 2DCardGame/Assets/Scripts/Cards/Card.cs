using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] public string cardName;
    [SerializeField] public Cards cardType;
    [TextArea(5, 10)] [SerializeField] public string description;

    public override string ToString()
    {
        return cardType.ToString();
    }
}
