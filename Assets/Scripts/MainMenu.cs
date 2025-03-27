using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject LevelSelect;

    [Header("Camera Movement")]
    public Camera mainCamera; 
    public float moveSpeed = 5f; 
    bool isAtStartMenu = true;

    public void StartGame() {
        Debug.Log("Go to level_1");
        // SceneManager.LoadScene("Level_1");
    }

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the editor
        #else
            Application.Quit(); // Quits the application
        #endif
    }

    public void ToggleMenu() {
        if (isAtStartMenu) {
            StartCoroutine(MoveCamera(new Vector3(63f, 0f, 0f)));
            StartMenu.SetActive(false);
        } else {
            StartCoroutine(MoveCamera(new Vector3(-63f, 0f, 0f)));
            LevelSelect.SetActive(true);
        }

        isAtStartMenu = !isAtStartMenu;
    }

    private IEnumerator MoveCamera(Vector3 offset)
    {
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = startPos + offset;
        float elapsedTime = 0f;
        float duration = Mathf.Abs(offset.x) / moveSpeed;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPos; // Ensure exact final position

        if (!isAtStartMenu) LevelSelect.SetActive(true);
        else StartMenu.SetActive(true);
    }

    public void SelectLevel(Button button) {
        string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text;
        if (buttonText == "Level 1") {
            Debug.Log("Level 1!");
        } else if (buttonText == "Level 2") {
            Debug.Log("Level 2!");
        } else if (buttonText == "Level 3") {
            Debug.Log("Level 3!");
        }
    }

    public void Tutorial() {
        Debug.Log("Display Tutorial");
    }
}
