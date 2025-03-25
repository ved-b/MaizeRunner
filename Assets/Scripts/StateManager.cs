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
            Application.Quit();
            return;
        }
        if (name != null){
            SceneManager.LoadScene(name);
        }
        
    }
}
