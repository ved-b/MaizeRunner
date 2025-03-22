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

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver(){
        if (active) {
            if (Input.GetMouseButtonDown(0)) {
                ClickedTile();
            } else if (Input.GetMouseButtonDown(1)) {
                flagged = !flagged;
                if (flagged) {
                    spriteRenderer.sprite = FlaggedTile;
                } else {
                    spriteRenderer.sprite = unclickedTile;
                }
            }
        }
    }

    public void ClickedTile() {
        if (active & !flagged) {
            active = false;
            if (isMine) {
                spriteRenderer.sprite = mineHitTile;
                gameManager.GameOver();
            } else {
                spriteRenderer.sprite = clickedTiles[mineCount];
                if (mineCount == 0) {
                    gameManager.ClickNeighbours(this);
                }
            }
        }
    }

     private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && active)
        {
            ClickedTile();
        }
    }
}
