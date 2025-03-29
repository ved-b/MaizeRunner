using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraZoom : SingletonMonoBehavior<CameraZoom>
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera followCamera;
    [SerializeField] private CinemachineCamera mainCamera;
    
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
        }
        else
        {
            // Switch to follow camera
            followCamera.Priority = 10;
            mainCamera.Priority = 0;
            Debug.Log("Switched to Follow Camera");
        }
        
        isUsingFollowCamera = !isUsingFollowCamera;
    }
}