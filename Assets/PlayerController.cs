using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Jumping")]
    [SerializeField] private int extraJumps = 1; // number of mid-air jumps (double jump = 1)

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck; // empty child transform positioned at feet
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    private bool isDashing;
    private float dashTimeLeft;
    private float dashCooldownTimer;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private bool facingRight = true;
    private int extraJumpsLeft;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        extraJumpsLeft = extraJumps;
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        HandleCollision();


        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            extraJumpsLeft = extraJumps;
        }


        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                DoJump();
            }
            else if (extraJumpsLeft > 0)
            {
                DoJump();
                extraJumpsLeft--;
            }
        }


        if (horizontal > 0.01f && !facingRight) Flip();
        else if (horizontal < -0.01f && facingRight) Flip();

        // Dash input (LeftShift tuþu örnek)
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            dashCooldownTimer = dashCooldown;
        }

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
            float dashDir = facingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);

            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
            }
            return; // dashing sýrasýnda normal hareketi engelle
        }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        HandleAnimations();
    }


    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }
    }
}
