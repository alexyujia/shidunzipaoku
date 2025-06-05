using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject PausePanel;
    public AudioSource BGM;

    void Start() {
        PausePanel.SetActive(false);
    }

    public void TogglePause(){
        if(PausePanel.activeSelf){
            Time.timeScale = 1;
            BGM.Play();
        } else {
            Time.timeScale = 0;
            BGM.Pause();
        }
        PausePanel.SetActive(!PausePanel.activeSelf);
    }
}
