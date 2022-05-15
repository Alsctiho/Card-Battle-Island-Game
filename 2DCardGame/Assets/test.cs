using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private static GameObject cardPrefab = null;
    private static bool mylock = true;

    private void Awake()
    {
        cardPrefab = Resources.Load("Card") as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(mylock)
        {
            //Card card = CardFactory.GetCard(null);
            mylock = false;
        }
    }
}
