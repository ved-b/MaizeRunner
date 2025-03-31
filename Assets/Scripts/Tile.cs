using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite FlaggedTile;
    [SerializeField] private List<Sprite> clickedTiles;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite mineWrongTile;
    [SerializeField] private Sprite mineHitTile;

    [Header("GM set via code")]
    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;
    public bool isWinner = false;
    private static bool isFollowCameraActive = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
{
    // when the follow camera is NOT active
    if (!isFollowCameraActive && active)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Bounds tileBounds = GetComponent<Collider2D>().bounds;
            if (tileBounds.Contains(mousePosition))
            {
                ToggleFlag();
                Debug.Log("Flag toggled from main camera view");
            }
        }
    }
}

    private void OnMouseOver(){
        if (active && isFollowCameraActive) {
            if (Input.GetMouseButtonDown(1)) {
                ToggleFlag();
            }
        }
    }

    public void ToggleFlag() 
    {
        if (active) {
            flagged = !flagged;
            if (flagged) {
                spriteRenderer.sprite = FlaggedTile;
            } else {
                spriteRenderer.sprite = unclickedTile;
            }
        }
    }

    public static void SetActiveCamera(bool isFollowCamera)
    {
        isFollowCameraActive = isFollowCamera;
    }

    public void ClickedTile(bool forceReveal = false) {
        if (active && (forceReveal || (!flagged || isMine))) {
            active = false;
            
            if (isMine) {
                spriteRenderer.sprite = mineHitTile;
                GridMovement gm = GameObject.FindWithTag("Player").GetComponent<GridMovement>();
                if (gm != null){
                    gm.HitMine();
                }
            } else {
                AudioManager.Instance.Play("Clear");
                spriteRenderer.sprite = clickedTiles[mineCount];
                if (mineCount == 0) {
                    gameManager.ClickNeighbours(this);
                }
            }
        }     
    }

    public void Deactivate(){
        if (active & !flagged) {
            active = false;
            
            if (isMine) {
                spriteRenderer.sprite = mineHitTile;
            } else {
                spriteRenderer.sprite = clickedTiles[mineCount];
                if (mineCount == 0) {
                    gameManager.ClickNeighbours(this);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && active) {
            ClickedTile(true);
        }
        if (other.CompareTag("Player") && isWinner) 
        {
            gameManager.WinGame();
        }
    
    }
}   
