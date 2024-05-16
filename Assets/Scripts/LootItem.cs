using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//thuoc tinh giup class nay se thao tac duoc tren inspector va tuan tu du lieu
public class LootItem
{
    public GameObject itemPrefab;
    [Range(0, 100)] public float dropChance;//range de gioi han gia tri cua dropChance, co the tuy chinh keo ngang trong unity editor

}
