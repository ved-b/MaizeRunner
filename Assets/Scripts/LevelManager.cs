using UnityEngine;
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
        } else {
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
        }
    }

}
