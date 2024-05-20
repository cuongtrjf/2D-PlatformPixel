using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    bool isCollect;
    private void Start()
    {
        isCollect = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InterfaceItem interfaceItem = collision.GetComponent<InterfaceItem>();
        //duoc hieu la khi collector cua player co collider tiep xuc voi 1 collider khac
        //ma co component chua interfaceItem thi se goi collect cua item do
        if(interfaceItem != null && !isCollect)
        {
            interfaceItem.Collect();
            isCollect = true;
        }
        StartCoroutine(SetStateCollect());
    }

    IEnumerator SetStateCollect()
    {
        yield return new WaitForSeconds(0.00001f);
        isCollect = false;
    }
}
