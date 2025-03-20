using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject LevelSelect;

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

    public void SelectLevelMenu() {
        if (StartMenu.activeSelf) {
            //Disable StartMenu Screen
            StartMenu.SetActive(false);

            //Enable LevelSelect
            LevelSelect.SetActive(true);
        }
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

    public void ReturnStartMenu () {
        if (LevelSelect.activeSelf) {
            //Disable LevelSelect
            LevelSelect.SetActive(false);

            //Enable StartMenu
            StartMenu.SetActive(true);
        }
    }
}
