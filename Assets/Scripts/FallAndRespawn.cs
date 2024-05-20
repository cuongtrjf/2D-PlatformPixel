using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndRespawn : MonoBehaviour
{
    public GameObject fallingPlatformPrefab;
    public float timeReSpawn = 1.5f;
    private GameObject currentObject;
    private bool stateSpawn;

    void Start()
    {
        // Khoi tao doi tuong 
        CreateObject();
    }

    void CreateObject()
    {
        stateSpawn = true;
        currentObject = Instantiate(fallingPlatformPrefab, transform.position, Quaternion.identity);
        currentObject.transform.parent = transform;
        // ??ng ký s? ki?n khi ??i t??ng b? h?y
        //ObjectDestroyer destroyer = currentObject.AddComponent<ObjectDestroyer>();
        //if(destroyer != null)
        //{
        //    destroyer.onDestroyEvent += OnObjectDestroyed;
        //}
    }

    private void Update()
    {
        if (currentObject == null && stateSpawn && transform.parent != null)
        {
            StartCoroutine(RecreateObject());
        }
    }
    //void OnObjectDestroyed()
    //{
    //    StartCoroutine(RecreateObject());
    //}

    IEnumerator RecreateObject()
    {
        stateSpawn = false;
        yield return new WaitForSeconds(timeReSpawn);
        CreateObject();
    }
}

//public class ObjectDestroyer : MonoBehaviour
//{
//    public delegate void OnDestroyEvent();
//    public event OnDestroyEvent onDestroyEvent;

//    void OnDestroy()
//    {
//        if (onDestroyEvent != null)
//        {
//            onDestroyEvent();
//        }
//    }
//}
