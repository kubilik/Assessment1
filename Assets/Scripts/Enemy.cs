using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Düþmanýn hareket hýzý

    public Transform player;           // Player referansý
    public float detectionRange = 5f;  // Player'ý algýlama mesafesi
    private bool facingRight = false;   // Düþmanýn yönü
    private int facingDirection => facingRight ? -1 : 1;

    [Header("Ground Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f; // Yerden kontrol mesafesi
    [SerializeField] private LayerMask whatIsGround;    // Yerin ne olduðunu belirlemek için katman maskesi
    private bool isGrounded;          // Düþmanýn yerde olup olmadýðýný kontrol etmek için

    void Update()
    {
        Groundcheck();
        if (!isGrounded) Flip(); // Havadaysa yönünü deðiþtir



        transform.position += Vector3.left * speed * facingDirection * Time.deltaTime;

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
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

            // Düþman, player'a doðru hareket etsin
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
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

    // Editor içinde detection range’ini görselleþtir
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

    }
}
