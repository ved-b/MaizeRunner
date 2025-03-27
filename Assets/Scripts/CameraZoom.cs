using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CinemachineCameraZoom : MonoBehaviour
{
    [Header("Cinemachine Camera Reference")]
    [Tooltip("Drag the CinemachineCamera component here if it is on a different GameObject.")]
    public CinemachineCamera cinemachineCam;

    [Header("Zoom Settings")]
    [Tooltip("Target orthographic size to zoom to.")]
    public float targetOrthographicSize = 2.5f;

    [Tooltip("How long the zoom should take, in seconds.")]
    public float zoomDuration = 2f;

    private void Start()
    {
        // Attempt to find the CinemachineCamera if not assigned
        if (cinemachineCam == null)
        {
            cinemachineCam = GetComponent<CinemachineCamera>();
            if (cinemachineCam == null)
            {
                Debug.LogError("No CinemachineCamera found on this GameObject. " +
                               "Please assign one in the Inspector or place this script " +
                               "on the same GameObject that has a CinemachineCamera component.");
                return;
            }
        }

        // Ensure the camera is set to Orthographic
        //cinemachineCam.Lens.Orthographic = true;

        // Tween the OrthographicSize property from its current value to the target value
        DOTween.To(
            () => cinemachineCam.Lens.OrthographicSize,       // Getter: current size
            x  => cinemachineCam.Lens.OrthographicSize = x,   // Setter: new size
            targetOrthographicSize,                           // Target value
            zoomDuration                                      // Duration in seconds
        )
        // Optional: Add any additional DOTween settings or easing here
        .SetEase(Ease.OutCubic);
    }
}
