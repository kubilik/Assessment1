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
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float comboResetTime = 1.5f; // kaç saniye sonra resetlenecek
    [Space]
    [SerializeField] private int ComboAttackDamage1;
    [SerializeField] private int ComboAttackDamage2;
    [SerializeField] private int ComboAttackDamage3;

    private AttackHitbox hitboxScript;
    private int comboStep = 1;
    private float comboResetTimer = 0f;
    private bool canAttack;

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

        // Eðer kombodayken zaman dolarsa resetle
        if (comboStep > 0)
        {
            comboResetTimer -= Time.deltaTime;
            if (comboResetTimer <= 0f)
            {
                comboStep = 1;
                anim.SetInteger("ComboCounter", comboStep);
                Debug.Log("Combo resetlendi!");
            }
        }

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    if (canAttack)
        //        anim.SetBool("Attack", true);
        //} 
    }
    public void SetCanAttack(bool state)
    {
        canAttack = state;
    }

    // Bu fonksiyon animasyon event’inden çaðrýlacak
    public void EnableHitbox()
    {
        // Kombo hasarýný ayarla
        int damage = 1;
        if (comboStep == 1) damage = ComboAttackDamage1;
        else if (comboStep == 2) damage = ComboAttackDamage2;
        else if (comboStep == 3) damage = ComboAttackDamage3;

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
        if (comboStep > 3) comboStep = 1; // 3. saldýrýdan sonra baþa dön
        anim.SetInteger("ComboCounter", comboStep);

        comboResetTimer = comboResetTime; // süreyi sýfýrla
        Debug.Log("Combo Step: " + (comboStep));
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


    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }
    }
}
