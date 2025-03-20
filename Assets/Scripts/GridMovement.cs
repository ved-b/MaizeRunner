using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private int gridWidth = 16;
    [SerializeField] private int gridHeight = 13;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-8f, -4f);
    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMove(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMove(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMove(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMove(Vector2.right);
            }
        }
    }
    
    private void TryMove(Vector2 direction)
    {
        // Calculate the target position
        Vector2 targetPosition = (Vector2)transform.position + direction * gridSize;
        
        // Check if the target position is within the grid
        if (IsValidPosition(targetPosition))
        {
            StartCoroutine(MovePlayer(direction));
        }
    }
    
    private bool IsValidPosition(Vector2 position)
    {
        // Check if the position is within the grid boundaries
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

        // Note where the player is currently, and where they are going
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction * gridSize;

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            // Move the player towards the end position
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percentage);
            yield return null;
        }

        // Ensure the player is at the end position
        transform.position = endPosition;

        isMoving = false;
    }
}
