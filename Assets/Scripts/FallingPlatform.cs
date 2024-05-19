using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallingWait = 0.5f;
    public float destroyWait = 1f;
    bool isFalling;
    Rigidbody2D rb;
    PlayerMovement PlayerMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player") && PlayerMovement.isGround)
        {
            StartCoroutine(Fall());
        }
    }


    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallingWait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyWait);
    }
}
