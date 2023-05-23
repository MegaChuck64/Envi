using System.Collections;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    public void Hit()
    {
        Debug.Log("Hit");
        

        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(Reactivate());        
    }

    private IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(0.5f);

        GetComponent<BoxCollider2D>().enabled = true;
    }
    
    
}
