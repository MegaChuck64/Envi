using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float health = 100f;
    public float hoverOffset = 0.4f;
    public float moveSpeed;
    public float attackDamage;
    public Vector2 target;
    
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //make ghost hover like a sin wave at target
        transform.position = new Vector2(transform.position.x, target.y + Mathf.Sin(Time.time * moveSpeed) * hoverOffset);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControls>().TakeDamage(attackDamage);
        }
    }
}
