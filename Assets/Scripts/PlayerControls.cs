using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public TriggerEventHandler feet;
    public Tilemap tileMap;
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    public bool _isGrounded;

    private bool doorEntered;
    private Rect doorRect;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        feet.onTriggerEnterEvent += Feet_onTriggerEnterEvent;
        feet.onTriggerExitEvent += Feet_onTriggerExitEvent;
    }

    private void Feet_onTriggerExitEvent(Collider2D obj)
    {
        _isGrounded = false;
    }

    private void Feet_onTriggerEnterEvent(Collider2D obj)
    {
        _isGrounded = true;
    }

    void Update()
    {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed);
        }

        var moveVelocity = 0f;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = -moveSpeed;
            _spriteRenderer.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = moveSpeed;
            _spriteRenderer.flipX = false;
        }

        _rigidbody2D.velocity = new Vector2(moveVelocity, _rigidbody2D.velocity.y);        
        
        if (doorEntered)
        {
            //check if boxcollider2d overlaps with doorrect            
            if (!doorRect.Overlaps(GetPlayerRect(), true))
            {
                doorEntered = false;
            }
            else
            {                                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    var currentScene = int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]);
                    SceneManager.LoadScene("Level " + (currentScene + 1), LoadSceneMode.Single);
                }                
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var point in collision.contacts)
        {
            var tile = tileMap.GetTile(tileMap.WorldToCell(point.point));
            if (tile != null)
            {
                if (!_isGrounded)
                {
                    if (tile.name.StartsWith("question_block"))
                    {
                        var rect = GetTileRect(point.point);
                        var playerRect = GetPlayerRect();

                        //if difference between player ymax and tile ymin are within .05
                        //we likely came from below
                        if (playerRect.yMax - rect.yMin <= .05f)
                        {

                            //MoveTileBaseToAndFromOverTime make sure to offset by tilemap size
                            StartCoroutine(
                                MoveTileBaseToAndFromOverTime(
                                    tile,
                                    tileMap.WorldToCell(point.point),
                                    tileMap.WorldToCell(point.point) + new Vector3(0, 0.2f, 0),
                                    0.1f));

                            //check for grass grower gameObject above question and call 
                            var grassGrower =
                                Physics2D.OverlapBoxAll(
                                    new Vector2(rect.x, rect.y + 1),
                                    new Vector2(1,1),
                                    0f).FirstOrDefault(t=>t.GetComponent<GrassGrower>() != null);


                            if (grassGrower?.GetComponent<GrassGrower>() is GrassGrower grss)
                            {
                                grassGrower.transform.SetParent(point.collider.transform);
                                grss.Grow();
                                tileMap.SetTile(tileMap.WorldToCell(point.point), grss.usedTile);
                            }

                        }
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (var point in collision.contacts)
        {
            var tile = tileMap.GetTile(tileMap.WorldToCell(point.point));
            if (tile != null)
            {
                if (tile.name.StartsWith("door"))
                {                    
                    //remove collider
                    tileMap.SetColliderType(tileMap.WorldToCell(point.point), Tile.ColliderType.None);
                    doorRect = GetTileRect(point.point);
                    doorEntered = true;
                }
            }
        }
    }

    private Rect GetTileRect(Vector2 point) => new Rect(point, Vector2.one);
    private Rect GetPlayerRect() => 
        new Rect(
            transform.position.x - 0.5f,
            transform.position.y - 0.5f,
            1f, 
            1f);
    

    private IEnumerator MoveTileBaseToAndFromOverTime(TileBase tile, Vector3Int startPosition, Vector3 endPosition, float time)
    {

        var xOffset = 14;
        //move tile incremently over time using lerp until we reach the end position        

        var elapsedTime = 0f;

        while (elapsedTime < time)
        {
            tileMap.SetTransformMatrix(
                startPosition, 
                Matrix4x4.TRS(
                    Vector3.Lerp(
                        startPosition - new Vector3(tileMap.cellSize.x * xOffset, tileMap.cellSize.y, 0),
                        endPosition - new Vector3(tileMap.cellSize.x * xOffset, tileMap.cellSize.y, 0),
                        elapsedTime / time), 
                    Quaternion.identity, 
                    Vector3.one));
            
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < time)
        {
            tileMap.SetTransformMatrix(
                startPosition,
                Matrix4x4.TRS(
                    Vector3.Lerp(
                        endPosition - new Vector3(tileMap.cellSize.x * xOffset, tileMap.cellSize.y, 0),
                        startPosition - new Vector3(tileMap.cellSize.x * xOffset, tileMap.cellSize.y, 0),
                        elapsedTime / time),
                    Quaternion.identity,
                    Vector3.one));


            elapsedTime += Time.deltaTime;

            yield return null;
        }

    }


}
