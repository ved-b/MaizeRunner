using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void ReloadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSceneByName(string name){
        if (name == "Quit"){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
            return;
        }
        if (name != null){
            SceneManager.LoadScene(name);
        }

        if (name == "Level2"){
            StartCoroutine(WaitAndLoadNextLevel("Level 2"));
        }

        if (name == "Level3"){
            StartCoroutine(WaitAndLoadNextLevel("Level 3"));
        }

        if (name == "MainMenu"){
            StartCoroutine(WaitAndLoadNextLevel("MainMenu"));
        }
        
    }

    private IEnumerator WaitAndLoadNextLevel(string name){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(name);
    }
}
