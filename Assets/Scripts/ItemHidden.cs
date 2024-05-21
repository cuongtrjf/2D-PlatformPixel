using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHidden : MonoBehaviour
{
    public GameObject heartHidden;

    private void Start()
    {
        Instantiate(heartHidden,transform.position,Quaternion.identity);
        GameController.OnReset += Spawn;
        GameController.OnRestart += Spawn;
    }

    void Spawn()
    {
        if (gameObject != null)
            return;
        Instantiate(heartHidden, transform.position, Quaternion.identity);
    }
}
