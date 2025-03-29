using DG.Tweening;
using UnityEngine;

public class CameraShake : SingletonMonoBehavior<CameraShake>
{
    // This reference should be updated to point to the active camera.
    [SerializeField] private Transform activeCameraTransform;

    public void SetActiveCamera(Transform newCameraTransform)
    {
        activeCameraTransform = newCameraTransform;
    }

    public void Shake(float duration, float strength)
    {
        if (activeCameraTransform == null)
        {
            Debug.LogWarning("Active camera transform not set, defaulting to Camera.main");
            activeCameraTransform = Camera.main.transform;
        }
        
        activeCameraTransform.DOShakePosition(duration, strength);
        activeCameraTransform.DOShakeRotation(duration, strength);
    }
}
