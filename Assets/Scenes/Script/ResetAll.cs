using UnityEngine;

using UnityEngine.SceneManagement;

public class ResetAll : MonoBehaviour
{
    // void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }

    public void GameReset()
    {
        // if (NetworkClient.isConnected)
        // {
        Time.timeScale = 1;
        SceneManager.LoadScene("Running");
        // }
        // else
        // {
        //     SceneManager.LoadScene("Running");
        // }
    }

    public void GameExit()
    {
        Time.timeScale = 1;
        DataStorager.SaveStatus();
        SceneManager.LoadScene("Initalize");
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Running");
    }

    public void GoToShop(){
        SceneManager.LoadScene("Shop");
    }

    public void GoToSettings(){
        SceneManager.LoadScene("Settings");
    }

    public void GoToMusic(){
        SceneManager.LoadScene("MusicLobby");
    }
}
