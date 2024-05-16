using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//so 0 dai dien cho nut chuot trai
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 mousePosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);//chuyen vi tri chuot trong scence thanh vitri trong the gioi 3D

        Vector3 shootDirection = (mousePosition - transform.position).normalized;//chuan hoa thanh vector don vi de mo ta huong tu vitri chuon den player

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;//set toc do cua dan
        Destroy(bullet, 2f);
        
    }
}
