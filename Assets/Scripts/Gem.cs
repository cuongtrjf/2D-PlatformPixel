using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour,InterfaceItem
{
    //class nay de nhat item
    public static event Action<int> OnGemCollect;
    //day la 1 event co kieu du lieu la action int dc su dung khi gem dc nhat
    //static la vi co the goi o bat ki dau trong code
    public int worth = 5;
    
    public void Collect()
    {
        OnGemCollect.Invoke(worth);//khi gem dc nhat thi no truyen gia tri worth vao, invoke la de kich hoat event nhat gem
        Destroy(gameObject);//khi nhat thi xoa vat pham do di
    }
}
