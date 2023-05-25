using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float health = 100f;
    public float hoverOffset = 0.4f;
    public float moveSpeed;
    public float blinkSpeed;
    public float attackDamage;
    public Vector2 target;

    private void Start()
    {
        StartCoroutine(PingPongAlpha());
    }
    
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //make ghost hover like a sin wave at target
        transform.position = new Vector2(transform.position.x, target.y + Mathf.Sin(Time.time * moveSpeed) * hoverOffset);       
    }

    public IEnumerator PingPongAlpha ()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GetComponent<SpriteRenderer>().color.a >= 0.5f)
            {
                collision.GetComponent<PlayerControls>().TakeDamage(attackDamage);
            }
        }
    }
}
