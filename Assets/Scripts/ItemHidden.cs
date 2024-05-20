using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHidden : MonoBehaviour
{
    public GameObject heartHidden;

    private void Start()
    {
        Spawn();
        GameController.OnReset += Spawn;
    }

    void Spawn()
    {
        Instantiate(heartHidden, transform.position, Quaternion.identity);
    }
}
