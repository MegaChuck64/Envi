using UnityEngine;

public class Creature : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed;
    public float attackDamage;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

}
