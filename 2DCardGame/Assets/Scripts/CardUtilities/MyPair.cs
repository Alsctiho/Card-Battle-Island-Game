using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPair<T1, T2>
{
    public T1 first;
    public T2 second;

    public MyPair(T1 v1, T2 v2)
    {
        first = v1;
        second = v2;
    }
}
 