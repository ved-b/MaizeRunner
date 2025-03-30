using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject transitionPanel;
    public void ReloadCurrentScene(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
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
            StartCoroutine(LoadLevel(name));
        }
        
    }

    IEnumerator LoadLevel(string name){
        AudioManager.Instance.Stop("mainMenu");
        AudioManager.Instance.Play("Transition");
        transitionPanel.SetActive(true);
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(name);
        transitionAnim.SetTrigger("Start");
        AudioManager.Instance.Play("Transition");
    }
}
