using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isFacingRight = true;
    public Animator animator;

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
    private bool isGround;


    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;


    [Header("WallCheck")]
    //kiem tra mat dat
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;//tuong


    [Header("WallMovement")]
    //truot tuong
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;



    [Header("WallJumping")]
    //nhay khi dang truot tuong se bat sang huong khac
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);//di chuyen theo phuong ngang vi co trong luc
            Flip();
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

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
                animator.SetTrigger("jump");
            }else if (context.canceled)
            {//neu chi nhan nhay roi tha ra thi se nhay thap
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
                animator.SetTrigger("jump");
            }
        }

        //wall jump 
        if(context.performed && wallJumpTimer > 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            animator.SetTrigger("jump");
            //lat khi nhay tuong vi no cung nhay sang huong nguoc lai
            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;//lien quan den sprite
                ls.x *= -1f;//lat sprite
                transform.localScale = ls;
            }


            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }

    }


    private void GroundCheck()//ham check nhan vat dang o tren mat dat
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))//ham kiem tra co cham voi layermask ground khong
        {
            jumpRemaining = maxJump;
            isGround = true;
        }
        else
        {
            isGround = false;
        }
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


    private bool WallCheck()//ham check xem co dang truot tuong khong
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }



    private void ProcessWallSlide()//ham xu ly truot tuong
    {
        //dieu kien la khong cham dat, dang truot tuong, di chuyen lien tuc de bam tuong
        if(!isGround && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void ProcessWallJump()//ham xu li nhay khi dang bam tuong
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;//huong nhay nguoc lai voi huong dang huong vao tuong
            wallJumpTimer = wallJumpTime;


            CancelInvoke(nameof(CancelWallJump));
        }else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;//het thoi gian nhay 
        }
    }


    private void CancelWallJump()// neu thoi gian nhay het thi se k nhay
    {
        isWallJumping = false;
    }


    //ham lat khi huong di chuyen sang trai hoac phai thi check truot tuong cung lat theo
    //mac dinh la check phai
    private void Flip()
    {
        if(isFacingRight && horizontalMovement<0 || !isFacingRight && horizontalMovement > 0)// >0 la di chuyen sang phai va ngc lai
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;//lien quan den sprite
            ls.x *= -1f;//lat sprite
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()//ve 1 doi tuong gimoz k co that trong game
    {
        //check cham dat de nhay 2 lan
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position,groundCheckSize);

        //check cham tuong de truot tuong doc
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
