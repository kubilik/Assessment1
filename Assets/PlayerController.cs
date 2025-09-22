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


    [Header("Attack")]
    [SerializeField] private float AttackDamage = 10f;
    [SerializeField] private Transform attackPoint;
    private AttackHitbox hitboxScript;
    private int comboStep = 0;


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

    private void Start()
    {
        hitboxScript = attackPoint.GetComponent<AttackHitbox>();
        attackPoint.gameObject.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        HandleCollision();
        HandleJump();
        HandleDash();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("Attack", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("Attack", false);
        }
    }

    // Bu fonksiyon animasyon event’inden çaðrýlacak
    public void EnableHitbox()
    {
        // Kombo hasarýný ayarla
        int damage = 1;
        if (comboStep == 0) damage = 1;
        else if (comboStep == 1) damage = 2;
        else if (comboStep == 2) damage = 3;

        hitboxScript.damage = damage;
        attackPoint.gameObject.SetActive(true);
    }
    public void DisableHitbox()
    {
        attackPoint.gameObject.SetActive(false);
    }

    // Komboyu ilerletmek için (animasyon baþlarken çaðýrabilirsin)
    public void NextComboStep()
    {
        comboStep++;
        if (comboStep > 2) comboStep = 0; // 3. saldýrýdan sonra baþa dön
    }
    private void HandleDash()
    {
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

    private void HandleJump()
    {
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
    }

    void FixedUpdate()
    {
        bool flowControl = HandleFixedDash();
        if (!flowControl)
        {
            return;
        }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        HandleAnimations();
    }

    private bool HandleFixedDash()
    {
        if (isDashing)
        {
            float dashDir = facingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);

            dashTimeLeft -= Time.fixedDeltaTime;
            anim.SetBool("isDashing", true);
            if (dashTimeLeft <= 0f)
            {
                anim.SetBool("isDashing", false);
                isDashing = false;
            }
            return false; // dashing sýrasýnda normal hareketi engelle
        }

        return true;
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

    public bool ChangeDashingState(bool state)
    {
        isDashing = state;
        return isDashing;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy hit!");
            // Burada düþmana hasar verme kodunu ekleyebilirsiniz.
        }
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
