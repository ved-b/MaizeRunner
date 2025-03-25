using UnityEngine;
using System.Collections;
using DG.Tweening;

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
    [SerializeField] private Sprite[] upDeathSprites;
    [SerializeField] private Sprite[] downDeathSprites;
    [SerializeField] private Sprite[] leftDeathSprites;
    [SerializeField] private Sprite[] rightDeathSprites;
    [SerializeField] private Sprite[] upHurtSprites;
    [SerializeField] private Sprite[] downHurtSprites;
    [SerializeField] private Sprite[] leftHurtSprites;
    [SerializeField] private Sprite[] rightHurtSprites;
    [SerializeField] private float frameRate = 0.1f;
    [SerializeField] private float deathFrameRate = 0.1f;
    [SerializeField] private float moveDistance = 0.2f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float hurtFrameRate = 0.1f;
    public GameObject heart3, heart2, heart1;
    public int crowLives = 3;
    private bool isMoving = false;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Sprite[] currentSprites;
    private int currentFrame = 0;
    private float animationTimer = 0f;
    private Vector2 facingDirection = Vector2.down;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprites = downSprites;
        facingDirection = Vector2.down;
        if (currentSprites.Length > 0)
            spriteRenderer.sprite = currentSprites[0];
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isDead)
            return;
        animationTimer += Time.deltaTime;
        if (animationTimer >= frameRate && currentSprites.Length > 0)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (facingDirection != Vector2.up)
                {
                    facingDirection = Vector2.up;
                    currentSprites = upSprites;
                    ResetAnimation();
                }
                else
                {
                    TryMove(Vector2.up);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (facingDirection != Vector2.down)
                {
                    facingDirection = Vector2.down;
                    currentSprites = downSprites;
                    ResetAnimation();
                }
                else
                {
                    TryMove(Vector2.down);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (facingDirection != Vector2.left)
                {
                    facingDirection = Vector2.left;
                    currentSprites = leftSprites;
                    ResetAnimation();
                }
                else
                {
                    TryMove(Vector2.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (facingDirection != Vector2.right)
                {
                    facingDirection = Vector2.right;
                    currentSprites = rightSprites;
                    ResetAnimation();
                }
                else
                {
                    TryMove(Vector2.right);
                }
            }
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector2 targetPosition = (Vector2)transform.position + direction * gridSize;
        if (IsValidPosition(targetPosition))
            StartCoroutine(MovePlayer(direction));
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
            spriteRenderer.sprite = currentSprites[0];
    }

    private IEnumerator PlayHurtAnimation()
    {
        isMoving = true;
        
        Vector3 offset = Vector3.zero;
        if (facingDirection == Vector2.up)
            offset = new Vector3(0f, -moveDistance, 0f);
        else if (facingDirection == Vector2.down)
            offset = new Vector3(0f, moveDistance, 0f);
        else if (facingDirection == Vector2.left)
            offset = new Vector3(moveDistance, 0f, 0f);
        else if (facingDirection == Vector2.right)
            offset = new Vector3(-moveDistance, 0f, 0f);

        transform.DOMove(transform.position + offset, moveDuration);

        Sprite[] hurtSprites = downHurtSprites;
        if (facingDirection == Vector2.up)
            hurtSprites = upHurtSprites;
        else if (facingDirection == Vector2.down)
            hurtSprites = downHurtSprites;
        else if (facingDirection == Vector2.left)
            hurtSprites = leftHurtSprites;
        else if (facingDirection == Vector2.right)
            hurtSprites = rightHurtSprites;

        for (int i = 0; i < hurtSprites.Length; i++)
        {
            spriteRenderer.sprite = hurtSprites[i];
            yield return new WaitForSeconds(hurtFrameRate);
        }
        
        ResetAnimation();
        isMoving = false;
    }


    public void HitMine()
    {
        if(isDead)
            return;
        Debug.Log("Mine triggered! Revealing mine.");

        if (Camera.main != null)
        {
            CameraShake.Instance.Shake(1f, 1f);
        }

        crowLives--;
        Debug.Log("Crow lives remaining: " + crowLives);

        if (crowLives <= 0)
        {
            Die();
        }
        if (crowLives == 2)
        {
            heart3.gameObject.SetActive(false);
            AudioManager.Instance.SetPitch("Background", 1.05f);
        }
        if (crowLives == 1)
        {
            heart3.gameObject.SetActive(false);
            heart2.gameObject.SetActive(false);
            AudioManager.Instance.SetPitch("Background", 1.1f);
        }
    }
    public void Die()
    {
        isDead = true;
        StopAllCoroutines();
        StartCoroutine(PlayDeathAnimation());
        AudioManager.Instance.SetVolume("Background", 0.5f);
        AudioManager.Instance.SetPitch("Background", 0.5f);
        AudioManager.Instance.Play("Lose");
        heart3.gameObject.SetActive(false);
        heart2.gameObject.SetActive(false);
        heart1.gameObject.SetActive(false);
        PlayerDied();
    }

    private void PlayerDied(){
        LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }

    private IEnumerator PlayDeathAnimation()
    {
        Vector3 offset = Vector3.zero;
        if (facingDirection == Vector2.up)
            offset = new Vector3(0f, -moveDistance, 0f);
        else if (facingDirection == Vector2.down)
            offset = new Vector3(0f, moveDistance, 0f);
        else if (facingDirection == Vector2.left)
            offset = new Vector3(moveDistance, 0f, 0f);
        else if (facingDirection == Vector2.right)
            offset = new Vector3(-moveDistance, 0f, 0f);

        transform.DOMove(transform.position + offset, moveDuration);

        Sprite[] selectedDeathSprites = downDeathSprites;
        if (facingDirection == Vector2.up)
            selectedDeathSprites = upDeathSprites;
        else if (facingDirection == Vector2.down)
            selectedDeathSprites = downDeathSprites;
        else if (facingDirection == Vector2.left)
            selectedDeathSprites = leftDeathSprites;
        else if (facingDirection == Vector2.right)
            selectedDeathSprites = rightDeathSprites;

        for (int i = 0; i < selectedDeathSprites.Length; i++)
        {
            spriteRenderer.sprite = selectedDeathSprites[i];
            yield return new WaitForSeconds(deathFrameRate);
        }
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}
