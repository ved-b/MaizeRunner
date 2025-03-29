using UnityEngine;
using UnityEngine.SceneManagement;  // Needed for scene management
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (LevelManager.instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver(){
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null){
            _ui.ToggleDeathPanel();
        }
    }

    public void Win(){
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null){
            _ui.ToggleWinPanel();
            
            // Check if we're in level one (scene name "Level1")
            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                // Start the coroutine that waits for 10 seconds before loading level 2
                StartCoroutine(WaitAndLoadNextLevel());
            }
        }
    }

    private IEnumerator WaitAndLoadNextLevel(){
        // Wait for 10 seconds
        yield return new WaitForSeconds(2f);
        // Load the scene called "Level2"
        SceneManager.LoadScene("Level 2");
    }
}
