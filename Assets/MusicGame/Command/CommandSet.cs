using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandSet : MonoBehaviour
{
    public void Retry(){
        SceneManager.LoadScene("MusicGame");
    }

    public void Exit(){
        SceneManager.LoadScene("MusicLobby");
    }

    public void GoToStart(){
        SceneManager.LoadScene("Start");
    }
}
