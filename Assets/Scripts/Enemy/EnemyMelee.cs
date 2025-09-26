using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float AttackDamage = 5f;

    [Header("Movement")]
    [SerializeField] private float idleSpeed = 1f;  
    [SerializeField] private float detectionRange = 5f;  
    [SerializeField] private float attackRange = 5f;   
    [SerializeField] private float RunSpeed = 2f;  

    [Header("Ground Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;  
    [SerializeField] private LayerMask whatIsGround;     

    private Animator anim;
    private Transform player;            
    private Player PlayerController;      
    private int facingDirection => facingRight ? -1 : 1;
    private bool facingRight = false;    
    private bool isGrounded;           
    private bool alert;
    private bool dead;
    private bool attack;
    private bool run;
    private bool idle;
    private bool hit;

    public float playerDistance;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerController = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (dead) return;
        Groundcheck();

        if (PlayerController.isDead)
        {
            if (!isGrounded) Flip();  
            transform.position += Vector3.left * idleSpeed * facingDirection * Time.deltaTime;
            return;
        }


        bool flowControl = MovementControl();
        if (!flowControl)
        {
            return;
        }
    }

    private bool MovementControl()
    {
        if (!isGrounded) Flip();  
        playerDistance = Vector2.Distance(transform.position, player.position);


        if (playerDistance <= detectionRange && playerDistance > attackRange)
        {
            if (run == false && alert == false)
            {
                alert = true;
                anim.SetBool("Alert", alert);
                idle = false;
                anim.SetBool("Idle", idle);
                return false;  
            }

             
            if (player.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && facingRight)
            {
                Flip();
            }
             
            if (isGrounded)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * RunSpeed * Time.deltaTime;
            }
        }
        else if (playerDistance > detectionRange)
        {
            run = false;
            anim.SetBool("Run", run);
            idle = true;
            anim.SetBool("Idle", idle);
        }
        if (playerDistance <= attackRange)
        { 
            if (player.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && facingRight)
            {
                Flip();
            }

            run = false;
            anim.SetBool("Run", run);
            idle = false;
            anim.SetBool("Idle", idle);
            attack = true;
            anim.SetBool("Attack", attack);
        }

        if (isGrounded && !run && !attack)
            transform.position += Vector3.left * idleSpeed * facingDirection * Time.deltaTime;
        return true;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;  
        transform.localScale = scale;
    }

    private void Groundcheck()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }


    public void ChangeAlertToFalse()
    {
        alert = false;
        anim.SetBool("Alert", alert);

        run = true;
        anim.SetBool("Run", run);
    }

    public void ChangeAttackToFalse()
    {
        attack = false;
        anim.SetBool("Attack", attack);
    }

    public float GetAttackDamage()
    {
        return AttackDamage;
    }

    public void ChangeEnemyAnimHitToFalse()
    {
        anim.SetBool("Hit", false);
    }

    public void ChangeEnemyAnimDeadTo(bool state)
    {
        anim.SetBool("Dead", state);
    }

    public void SetDeadBool()
    {
        dead = true;
    }


    public void SetDead()
    {
        Destroy(gameObject); 
    }

     
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

    }
}
