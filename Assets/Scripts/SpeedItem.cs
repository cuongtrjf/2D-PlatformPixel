using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedItem : MonoBehaviour,InterfaceItem
{
    public float multiSpeed = 3f;
    public static event Action<float> OnSpeedCollected;


    public void Collect()
    {
        OnSpeedCollected.Invoke(multiSpeed);
        Destroy(gameObject);
    }
}
