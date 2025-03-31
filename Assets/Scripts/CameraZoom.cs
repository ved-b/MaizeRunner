using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraZoom : SingletonMonoBehavior<CameraZoom>
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera followCamera;
    [SerializeField] private CinemachineCamera mainCamera;

    [Header("InGame Canvas")]
    [SerializeField] private CanvasGroup inGameCanvas;
    [SerializeField] private float fadeSpeed = 5;
    
    private bool isUsingFollowCamera = true;
    
    
    private void Update()
    {
        // Check for F key press to switch cameras
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchCamera();
        }
    }
    
    public void SwitchCamera()
{
    if (followCamera == null || mainCamera == null)
    {
        Debug.LogError("Camera references not set in the Inspector");
        return;
    }
    
    if (isUsingFollowCamera)
    {
        // Switch to main camera
        followCamera.Priority = 0;
        mainCamera.Priority = 10;
        Debug.Log("Switched to Main Camera");
        CameraShake.Instance.SetActiveCamera(mainCamera.transform);
        FadeCanvas(inGameCanvas, 0.25f, fadeSpeed);
        
        Tile.SetActiveCamera(false);
    }
    else
    {
        // Switch to follow camera
        followCamera.Priority = 10;
        mainCamera.Priority = 0;
        Debug.Log("Switched to Follow Camera");
        CameraShake.Instance.SetActiveCamera(followCamera.transform);
        FadeCanvas(inGameCanvas, 1f, fadeSpeed);
        
        Tile.SetActiveCamera(true);
    }
    
    isUsingFollowCamera = !isUsingFollowCamera;
}

    public void FadeCanvas(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        canvasGroup.DOFade(targetAlpha, duration)
            .SetEase(Ease.InOutSine); 
    }
}