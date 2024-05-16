using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType
    {
        SmallGem,
        BigGem,
        Enemy
    }

    public Tilemap tilemap;
    public GameObject[] objectPrefabsList;//luu cac object vao day (smallgem, biggem, enemy)

    public float bigGemAbility = 0.2f;//kha nang xuat hien cua biggem
    public float enemyAbility = 0.1f;
    public int maxObjects = 5;//so luong object toi da dang xuat hien
    public float gemLifeTime = 10f;//thoi gian ton tai cua gem
    public float spawnInterval = 0.5f;//thoi gian spawn lai sau khi bien mat


    private List<Vector3> validSpawnPositionsList = new List<Vector3>();//cac vi tri spawn hop le
    private List<GameObject> spawnObjectsList = new List<GameObject>();//cac object de spawn
    private bool isSpawning = false;//trang thai spawn


    // Start is called before the first frame update
    void Start()
    {
        GatherValidPositions();//thu thap cac vi tri hop le
        StartCoroutine(SpawnObjectsIfNeeded());//spawn vao vi tri do
        GameController.OnReset += ChangeTileMap;//khi nhan reset thi cung nhu thay doi tilemap nen phai change
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemap.gameObject.activeInHierarchy)//neu tilemap dang lam viec k con active
        {
            //doi timemap khac, level khac de reset lai position spawn
            ChangeTileMap();
        }

        if(!isSpawning && ActiveObjectCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }


    private void ChangeTileMap()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();//reset lai vi tri co the spawn
        

        //pha huy tat ca cac object da spawn
        DestroyAllSpawnObjects();
    }



    //ham dem cac object dang active
    private int ActiveObjectCount()
    {
        spawnObjectsList.RemoveAll(item => item == null);//removeAll can 1 bieu thuc, bieu thuc o day la xoa neu item = null
        return spawnObjectsList.Count;
    }

    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;
        while(ActiveObjectCount() < maxObjects)
        {
            //neu chua du so object thi spawn sau khoang thoi gian spawnInterval
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }




    //SPAWN POSTION FOR OBJECT

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        //ham duoi kiem tra xem trong list spawnObject co phan tu nao thoa man dieu kien trong ngoac hay khong, tra ve true and false
        //bieu thuc trong ngoac la 1 bieu thuc lambla, no co nghia la
        //neu checkobject != null va khoang cach checkobject va position <1f, neu co object thoa man 2 dieu kien nay thi tra ve true
        return spawnObjectsList.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1f);
    }



    //randow spawn object type
    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;//lua chon ngau nhien tu 0 den 1
        if (randomChoice <= enemyAbility)
        {
            return ObjectType.Enemy;
        } else if (randomChoice <= (enemyAbility + bigGemAbility))
        {
            return ObjectType.BigGem;
        } else
            return ObjectType.SmallGem;
    }

    private void SpawnObject()
    {
        if (validSpawnPositionsList.Count == 0)//neu vi tri co the spawn bang 0 thi k spawn nua 
        {
            return;
        }

        Vector3 spawnPosition = Vector3.zero;//
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositionsList.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositionsList.Count);
            Vector3 potentialPos = validSpawnPositionsList[randomIndex];//vi tri tiem nang co the spawn
            Vector3 leftPosition = potentialPos + Vector3.left;
            Vector3 rightPosition = potentialPos + Vector3.right;//2 vi tri nay phuc vu check xem tai vi tri co the spawn da co object nao chua

            if(!PositionHasObject(leftPosition) && !PositionHasObject(rightPosition))
            {
                spawnPosition = potentialPos;
                validPositionFound = true;
            }
            validSpawnPositionsList.RemoveAt(randomIndex);//xoa vi tri vua dc spawn ra khoi list co the spawn
        }

        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectPrefabsList[(int)objectType], spawnPosition, Quaternion.identity);
            spawnObjectsList.Add(gameObject);
        
            //destroy object after time
            if(objectType!= ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, gemLifeTime));
            }
        }
    }



    private IEnumerator DestroyObjectAfterTime(GameObject gameObject,float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject)
        {
            spawnObjectsList.Remove(gameObject);
            validSpawnPositionsList.Add(gameObject.transform.position);
            Destroy(gameObject);
        }
    }





    private void DestroyAllSpawnObjects()
    {
        foreach(GameObject obj in spawnObjectsList)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnObjectsList.Clear();
    }






    //ham luu cac vi tri hop le de spawn object
    private void GatherValidPositions()
    {
        validSpawnPositionsList.Clear();//clear vi tri vi moi level lai la nhung vi tri khac nhau
        BoundsInt boundsInt = tilemap.cellBounds;//khoanh vung tilemap thanh 1 hinh chu nhat
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);//chuyen vung do thanh cac o 
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));//chuyen toa do khoanh vung sang toa do the gioi


        for(int x = 0; x < boundsInt.size.x; x++)//duyet qua cac o theo chieu ngang
        {
            for(int y = 0; y < boundsInt.size.y; y++)//duyet qua cac o theo chieu doc
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];//cong thuc nay anh xa thu tu cua 1 o trong mang voi toa do 2d
                //nom na la xet cac o trong mang theo toa do 2d tuong ung
                if (tile != null && y>0)
                {
                    //neu o do khong phai o rong thi vi tri xuat hien se nam sang phai 0.5f va tren 2f
                    Vector3 place = start + new Vector3(x + 0.5f, y + 1.5f, 0);
                    validSpawnPositionsList.Add(place);//them vi tri co the xuat hien
                }
            }
        }
    }



}
