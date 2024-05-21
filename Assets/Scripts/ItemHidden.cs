using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHidden : MonoBehaviour
{
    public GameObject heartHidden;
    private GameObject currentItem;
    private bool activeState;

    private void Start()
    {
        Create();
        GameController.OnReset += SetFalseState;
        GameController.OnRestart += SetFalseState;
    }
    private void Update()
    {
        if (!activeState && currentItem == null)
        {
            Create();
        }
    }

    void SetFalseState()//thiet lap state thay doi khi rest hay restart dc goi
    {
        activeState = false;
    }


    private void Create()
    {
        activeState = true;
        currentItem = Instantiate(heartHidden, transform.position, Quaternion.identity);
        currentItem.transform.parent = transform;//cap nhat lai vitri cua currentItem vi khi reset hoac restart, vi tri spawn se khac nhau theo map
    }
}
