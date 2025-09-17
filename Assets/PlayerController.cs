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
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashTimeLeft;
    private float dashCooldownTimer;

    private Rigidbody2D rb;
    private float horizontal;
    private bool facingRight = true;
    private int extraJumpsLeft;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsLeft = extraJumps;
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


        bool grounded = IsGrounded();
        if (grounded)
        {
            extraJumpsLeft = extraJumps;
        }


        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
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
    }


    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        Collider2D col = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return col != null;
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
