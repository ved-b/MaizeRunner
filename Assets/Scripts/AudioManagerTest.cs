using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        // Check if the AudioManager singleton instance is assigned
        if (AudioManager.instance != null)
        {
            // Log a message to the console
            Debug.Log("AudioManager singleton instance is working: " + AudioManager.instance);
            // Test playing a sound (e.g., "Background")
            AudioManager.instance.Play("Background");
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
