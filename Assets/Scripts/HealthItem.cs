using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthItem : MonoBehaviour,InterfaceItem
{
    public int healAmount = 1;
    public static event Action<int> OnHealthCollect;

    public void Collect()
    {
        OnHealthCollect.Invoke(healAmount);
        Destroy(gameObject);
    }

}
