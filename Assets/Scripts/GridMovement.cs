using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private int gridWidth = 16;
    [SerializeField] private int gridHeight = 13;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-8f, -4f);
    [SerializeField] private Sprite[] upSprites;
    [SerializeField] private Sprite[] downSprites;
    [SerializeField] private Sprite[] leftSprites;
    [SerializeField] private Sprite[] rightSprites;
    [SerializeField] private float frameRate = 0.1f; // Seconds per frame

    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Sprite[] currentSprites;
    private int currentFrame = 0;
    private float animationTimer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprites = downSprites;
        if (currentSprites.Length > 0)
        {
            spriteRenderer.sprite = currentSprites[0];
        }
    }

    private void Update()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= frameRate && currentSprites.Length > 0)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentSprites = upSprites;
                ResetAnimation();
                TryMove(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentSprites = downSprites;
                ResetAnimation();
                TryMove(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentSprites = leftSprites;
                ResetAnimation();
                TryMove(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentSprites = rightSprites;
                ResetAnimation();
                TryMove(Vector2.right);
            }
        }
    }
    
    private void TryMove(Vector2 direction)
    {
        Vector2 targetPosition = (Vector2)transform.position + direction * gridSize;
        if (IsValidPosition(targetPosition))
        {
            StartCoroutine(MovePlayer(direction));
        }
    }
    
    private bool IsValidPosition(Vector2 position)
    {
        float minX = gridOrigin.x;
        float maxX = gridOrigin.x + (gridWidth - 1) * gridSize;
        float minY = gridOrigin.y;
        float maxY = gridOrigin.y + (gridHeight - 1) * gridSize;
        
        return (position.x >= minX && position.x <= maxX &&
                position.y >= minY && position.y <= maxY);
    }

    private IEnumerator MovePlayer(Vector2 direction)
    {
        isMoving = true;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * gridSize;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percentage);
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;
        ResetAnimation();
    }
    
    private void ResetAnimation()
    {
        currentFrame = 0;
        animationTimer = 0f;
        if (currentSprites.Length > 0)
        {
            spriteRenderer.sprite = currentSprites[0];
        }
    }
}
