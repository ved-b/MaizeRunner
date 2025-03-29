using DG.Tweening;
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

    [Header("Tutorial Screen")]
    [SerializeField] private CanvasGroup tutorialCanvas;
    [SerializeField] private float fadeSpeed = 5;

    public void StartGame() {
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the editor
        #else
            Application.Quit(); // Quits the application
        #endif
    }

    public void ToggleMenu() {
        float moveDistance = 65.3f;
        if (isAtStartMenu) {
            MoveCamera(new Vector3(moveDistance, 0f, 0f));
            StartMenu.SetActive(false);
        } else {
            MoveCamera(new Vector3(-moveDistance, 0f, 0f));
            LevelSelect.SetActive(false);
        }

        isAtStartMenu = !isAtStartMenu;
    }

    private void MoveCamera(Vector3 offset)
    {
        Vector3 targetPos = mainCamera.transform.position + offset;
        float duration = Mathf.Abs(offset.x) / moveSpeed;

        mainCamera.transform.DOMove(targetPos, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                if (!isAtStartMenu) LevelSelect.SetActive(true);
                else StartMenu.SetActive(true);
            });

        // Vector3 startPos = mainCamera.transform.position;
        // Vector3 targetPos = startPos + offset;
        // float elapsedTime = 0f;
        // float duration = Mathf.Abs(offset.x) / moveSpeed;

        // while (elapsedTime < duration)
        // {
        //     mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        // mainCamera.transform.position = targetPos; // Ensure exact final position

        // if (!isAtStartMenu) LevelSelect.SetActive(true);
        // else StartMenu.SetActive(true);
    }

    public void SelectLevel(Button button) {
        string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text;
        if (buttonText == "Level 1") {
            SceneManager.LoadScene("Level 1");
        } else if (buttonText == "Level 2") {
            SceneManager.LoadScene("Level 2");
        } else if (buttonText == "Level 3") {
            SceneManager.LoadScene("Level 3");
        }
    }

    public void Tutorial() {
        if (tutorialCanvas.alpha == 0f) {
            tutorialCanvas.gameObject.SetActive(true);
            FadeCanvas(tutorialCanvas, 1f, fadeSpeed);
        }
    }

    public void ReturnToMenu() {
        if (tutorialCanvas.alpha == 1f) {   
            FadeCanvas(tutorialCanvas, 0f, fadeSpeed, () => {
                tutorialCanvas.gameObject.SetActive(false);
            });
        }
    }

    public void FadeCanvas(CanvasGroup canvasGroup, float targetAlpha, float duration, System.Action onComplete = null)
    {
        canvasGroup.DOFade(targetAlpha, duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => onComplete?.Invoke());
    }
}
