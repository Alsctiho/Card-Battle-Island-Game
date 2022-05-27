using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private GameObject cardPrefab = null;
    private bool mylock = true;

    private void Awake()
    {
        //cardPrefab = Resources.Load("Prefabs/SecondaryResource/Iron Variant") as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject cardObject = GameObject.Instantiate(cardPrefab);
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
