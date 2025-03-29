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
        // Not Needed
        // if (name == "Quit"){
        //     #if UNITY_EDITOR
        //         UnityEditor.EditorApplication.isPlaying = false;
        //     #else
        //         Application.Quit();
        //     #endif
        //     return;
        // }
        if (name != null){
            SceneManager.LoadScene(name);
        }
        
    }
}
