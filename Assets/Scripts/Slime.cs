using UnityEngine;

public class Slime : MonoBehaviour
{
    public float jumpSpeed = 5f;
    public float awarenessDistance = 5f;
    public float attackDamage = 5f;
    public float waitTime = 3f;
    
    private bool isMoving;
    private float waitTimer;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private PlayerControls player;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<PlayerControls>();
    }

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;            
        }
    }

    private void Update()
    {
        
        IsMoving = Vector2.Distance(player.transform.position, transform.position) < awarenessDistance;

        if (IsMoving)
        {
            sr.flipX = player.transform.position.x > transform.position.x;
            waitTimer += Time.deltaTime;

            if (waitTimer > waitTime)
            {
                waitTimer = 0f;
                JumpForward();
            }
        }
        else
        {
            //stop moving if grounded
            if (rb.velocity.y == 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void JumpForward()
    {
        var jumpDirection = sr.flipX ? Vector2.right : Vector2.left;
        jumpDirection.y = 1f;
        rb.AddForce(jumpDirection * jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(attackDamage);
        }
    }

}
