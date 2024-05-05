using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;//chuyen dong ngang

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJump = 2;
    private int jumpRemaining;


    [Header("GroundCheck")]
    //kiem tra mat dat
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;//mat dat


    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);//di chuyen theo phuong ngang vi co trong luc
        GroundCheck();
        Gravity();
    }


    //xy ly trong luc roi nhan vat khi nhay
    public void Gravity()
    {
        if (rb.velocity.y < 0)//khi nhan vat dang roi
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));//gioi han van toc roi cua nhan vat va vi tri roi
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }



    public void Move(InputAction.CallbackContext context)//khi co su nhap lieu se duoc goi context
    {
        horizontalMovement = context.ReadValue<Vector2>().x;//lay gia tri nhap lieu la 1 vector 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            if (context.performed)//neu thuc hien nhan phim
            {//neu giu nut nhay thi se nhay cao
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpRemaining--;
            }else if (context.canceled)
            {//neu chi nhan nhay roi tha ra thi se nhay thap
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
            }
        }
    }


    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))//ham kiem tra co cham voi layermask ground khong
        {
            jumpRemaining = maxJump;
        }
    }

    private void OnDrawGizmosSelected()//ve 1 doi tuong gimoz k co that trong game
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position,groundCheckSize);
    }
}
