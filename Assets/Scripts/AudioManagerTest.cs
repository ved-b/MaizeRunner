using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        // Check if the AudioManager singleton instance is assigned
        if (AudioManager.Instance != null)
        {
            // Log a message to the console
            Debug.Log("AudioManager singleton instance is working: " + AudioManager.Instance);
            // Test playing a sound (e.g., "Background")
            AudioManager.Instance.Play("Background");
        }
        else
        {
            Debug.LogError("AudioManager singleton instance not found!");
        }
    }

    void Update()
    {
        
    }
}
