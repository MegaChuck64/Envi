using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassGrower : MonoBehaviour
{
    public Sprite[] sprites;
    public float growTime;
    public Tile usedTile;
    
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[0];
    }

    public void Grow()
    {
        Debug.Log("Grow");
        StartCoroutine(GrowRoutine());
    }

    private System.Collections.IEnumerator GrowRoutine()
    {
        var currentSpriteIndex = 0;
        var timePerSprite = growTime / sprites.Length;

        while (currentSpriteIndex < sprites.Length)
        {
            _spriteRenderer.sprite = sprites[currentSpriteIndex];
            currentSpriteIndex++;
            yield return new WaitForSeconds(timePerSprite);            
        }

    }

}
