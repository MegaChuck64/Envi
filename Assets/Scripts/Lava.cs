using UnityEngine;

public class Lava : MonoBehaviour
{
    public float damage = 10f;
    private float damageRate = 1f;
    private float damageTimer;

    private PlayerControls playerController;

    private void Update()
    {       
        damageTimer += Time.deltaTime;
        
        if (damageTimer >= damageRate)
        {
            damageTimer = 0f;
            playerController?.TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerControls>() is PlayerControls player)
        {
            playerController = player;
            damageTimer = damageRate;
        }        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControls>() is PlayerControls player)
        {
            //lower player position slowly
            player.transform.position = 
                Vector3.Lerp(
                    player.transform.position, 
                    new Vector3(player.transform.position.x, player.transform.position.y - .5f, player.transform.position.z), 
                    0.1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerControls>() is PlayerControls player)
        {
            playerController = null;
        }
    }

    

}
