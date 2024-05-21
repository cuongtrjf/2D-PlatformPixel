using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;//duoi theo player
    public float chaseSpeed = 2f;//toc do duoi theo
    public float jumpForce = 2f;//luc nhay
    public LayerMask groundLayer;


    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
              
    public int damage = 1;


    //xu li enemy trung dan
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;


    //loot item
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();



      


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color; ;
    }

    // Update is called once per frame
    void Update()
    {
        //kiem tra cham dat
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        //raycast se la 1 tia chieu tu vi tri tranform theo huong xuong duoi voi khoang cach 1f check xem co xuyen qua groundlayer k


        //check huong cua nguoi choi
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        //ham Sign tra ve 1, -1 hoac 0 neu gia tri trong ngoac la duong, am hoac 0 <=> phai, trai, ko


        //check player co dang o tren khong
        bool isPlayerAbove = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.up, 5f, 1 << player.gameObject.layer);
        //vi sao phai dich bit, de tao ra layermask cho layer, layermask co cac bitmask de kiem tra hon


        if (isGrounded)
        {
            //duoi theo player
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);


            //enemy jump neu k co mat dat o truoc va khoang trong phia truoc
            //neu khong thi neu la nguoi choi o tren va mat dat o tren
            float directionVertical = Mathf.Sign(player.position.y - transform.position.y);
            bool diffVertical = directionVertical > 0;
            //mat dat phia truoc theo huong cua nguoi choi
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            //khoang trong
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            //neu khong co ca khoang trong va dat phia truoc thi check tuong o tren xem co khong
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0) * 3f, Vector2.up, 5f, groundLayer);

            if(!groundInFront.collider && !gapAhead.collider && diffVertical)
            {
                shouldJump = true;
            }else if(isPlayerAbove && platformAbove.collider && diffVertical)
            {
                shouldJump = true;
            }

        }

    }


    private void FixedUpdate()
    {
        if(isGrounded && shouldJump)
        {
            shouldJump = false;//khi dang nhay thi sholdjump se ve false
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;

            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);//impulse mo phong 1 luc tuc thoi nhanh va manh


        }
    }



    //xu ly enemy nhan dame tu player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashBlack());
        if (currentHealth <= 0)
        {
            //enemy die
            Die();
        }
    }

    private void Die()
    {
        //spawn item sau khi enemy chet
        foreach(LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLootItem(lootItem.itemPrefab);
                break;
            }
        }
        Destroy(gameObject);
    }


    private void InstantiateLootItem(GameObject lootItem)
    {
        if (lootItem)
        {
            GameObject dropItem = Instantiate(lootItem, transform.position, Quaternion.identity);
            dropItem.GetComponent<SpriteRenderer>().color = ogColor;
            Destroy(dropItem, 10f);
        }
    }

    private IEnumerator FlashBlack()
    {
        //Color color = spriteRenderer.color;
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }
}
