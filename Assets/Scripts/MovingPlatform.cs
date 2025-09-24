using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float speed = 2f;

    private Vector2 target;
    private Rigidbody2D rb;
    private Vector2 lastPos;
    private Vector2 platformVelocity;

    private Rigidbody2D playerRb;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (startPoint != null)
            rb.position = (Vector2)startPoint.position;

        target = (Vector2)endPoint.position;
        lastPos = rb.position;
    }

    void FixedUpdate()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        platformVelocity = (newPos - lastPos) / Time.fixedDeltaTime;
        lastPos = newPos;

        if (playerRb != null)
        {
            // yatay kayma
            playerRb.position += new Vector2(platformVelocity.x, 0f) * Time.fixedDeltaTime;

            // sadece yerdeyse VE zýplamýyorsa dikey velocity uygula
            if (playerController != null && playerController.IsGrounded())
            {
                // eðer oyuncu yukarý zýplýyorsa platform hýzýyla override etme
                if (playerRb.linearVelocity.y <= 0.01f)
                {
                    Vector2 v = playerRb.linearVelocity;
                    v.y = platformVelocity.y;
                    playerRb.linearVelocity = v;
                }
            }
        }

        if (Vector2.Distance(rb.position, target) < 0.05f)
        {
            target = (target == (Vector2)endPoint.position) ? (Vector2)startPoint.position : (Vector2)endPoint.position;

            // endpointte firlatmayi engelle
            if (playerRb != null && playerRb.linearVelocity.y <= 0.01f)
            {
                Vector2 v = playerRb.linearVelocity;
                v.y = 0f;
                playerRb.linearVelocity = v;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() == playerRb)
            {
                playerRb = null;
                playerController = null;
            }
        }
    }
}
