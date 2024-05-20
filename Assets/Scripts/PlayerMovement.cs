using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isFacingRight = true;
    public Animator animator;
    public ParticleSystem smokeFX;
    BoxCollider2D boxCollider;



    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;//chuyen dong ngang


    [Header("Dashing")]
    public float dashSpeed = 20f;//toc do dash
    public float dashDuration = 0.1f;//thoi gian dash
    public float dashCoolDown = 0.1f;//thoi gian hoi dash, tranh spam
    bool isDashing;
    bool canDash = true;
    TrailRenderer trailRenderer;//render hoat anh vet sau khi nvan di chuyen




    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJump = 2;
    public int jumpRemaining;


    [Header("GroundCheck")]
    //kiem tra mat dat
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;//mat dat
    public bool isGround;
    private bool underGround;


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
        trailRenderer = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);//xet cac dieu kien trong animator
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);


        if (isDashing)
        {
            return;//neu dang dash thi k the lam gi
        }

        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);//di chuyen theo phuong ngang vi co trong luc
            Flip();
        }
    }





    public void Move(InputAction.CallbackContext context)//khi co su nhap lieu se duoc goi context
    {
        horizontalMovement = context.ReadValue<Vector2>().x;//lay gia tri nhap lieu la 1 vector 
    }


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);//bo qua collision cua player(7) va enemy(8) khi va cham, neu la false thi se kich hoat lai
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;//emitting de bat tat render vet sau khi nhan vat di chuyen
        float dashDirection = isFacingRight ? 1f : -1f;//neu dang quay ben phai thi bang 1 con k la -1
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);
        rb.velocity = new Vector2(0f, rb.velocity.y);//reset lai van toc ngang
        isDashing = false;
        trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }



    //tuot xuong ground
    public void Drop(InputAction.CallbackContext context)
    {
        if(context.performed && isGround && !underGround)
        {
            StartCoroutine(DisableBoxCollider(0.25f));
        }
    }


    private IEnumerator DisableBoxCollider(float timeDisable)
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(timeDisable);
        boxCollider.enabled = true;
    }



    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            if (context.performed)//neu thuc hien nhan phim
            {//neu giu nut nhay thi se nhay cao
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpRemaining--;
                JumpFX();
            }else if (context.canceled)
            {//neu chi nhan nhay roi tha ra thi se nhay thap
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
                JumpFX();
            }
        }

        //wall jump 
        if(context.performed && wallJumpTimer > 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            JumpFX();
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
            if (transform.position.y > -4f)
            {
                underGround = false;
            }
            else
            {
                underGround = true;
            }
        }
        else
        {
            isGround = false;
        }
    }

    //xy ly trong luc roi nhan vat khi nhay
    public void Gravity()
    {
        if (rb.velocity.y < 0 && !isGround)//khi nhan vat dang roi
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
            wallJumpTimer -= Time.fixedDeltaTime;//het thoi gian nhay 
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

            if (rb.velocity.y == 0)
            {
                smokeFX.Play();
            }
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




    //effect
    private void JumpFX()
    {
        animator.SetTrigger("jump");
        smokeFX.Play();
    }
}
