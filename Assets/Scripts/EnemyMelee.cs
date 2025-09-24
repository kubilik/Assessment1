using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float AttackDamage = 5f;

    [Header("Movement")]
    [SerializeField] private float idleSpeed = 1f; // Düþmanýn hareket hýzý
    [SerializeField] private float detectionRange = 5f;  // Player'ý algýlama mesafesi
    [SerializeField] private float attackRange = 5f;  // Player'ý algýlama mesafesi
    [SerializeField] private float RunSpeed = 2f; // Düþmanýn hareket hýzý

    [Header("Ground Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f; // Yerden kontrol mesafesi
    [SerializeField] private LayerMask whatIsGround;    // Yerin ne olduðunu belirlemek için katman maskesi

    private Animator anim;
    private Transform player;           // Player referansý
    private int facingDirection => facingRight ? -1 : 1;
    private bool facingRight = false;   // Düþmanýn yönü
    private bool isGrounded;          // Düþmanýn yerde olup olmadýðýný kontrol etmek için
    private bool alert;
    private bool dead;
    private bool attack;
    private bool run;
    private bool idle;
    private bool hit;

    public float playerDistance;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Groundcheck();



        bool flowControl = MovementControl();
        if (!flowControl)
        {
            return;
        }
    }

    private bool MovementControl()
    {
        if (!isGrounded) Flip(); // Havadaysa yönünü deðiþtir
        playerDistance = Vector2.Distance(transform.position, player.position);


        if (playerDistance <= detectionRange && playerDistance > attackRange)
        {
            if (run == false && alert == false)
            {
                alert = true;
                anim.SetBool("Alert", alert);
                idle = false;
                anim.SetBool("Idle", idle);
                return false; // Alert animasyonu oynatýldýktan sonra bekle
            }

            // Player düþmanýn saðýnda mý solunda mý kontrol et
            if (player.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && facingRight)
            {
                Flip();
            }
            // Düþman, player'a doðru hareket etsin
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
            // Player düþmanýn saðýnda mý solunda mý kontrol et
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
        scale.x *= -1; // X ekseninde yansýtma
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

    // Editor içinde detection range’ini görselleþtir
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
