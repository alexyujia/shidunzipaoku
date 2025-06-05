using UnityEngine;
using UnityEngine.SceneManagement;

public class BuyItem : MonoBehaviour
{
    public AudioSource buySound;
    public AudioSource failSound;
    public void BuyLife(){
        int price = 100;
        if(DataStorager.coin.count >= price){
            DataStorager.coin.count -= price;
            DataStorager.SaveStatus();
            DataStorager.maxLife.count += 1;
            DataStorager.settings.CustomMaxLife = DataStorager.maxLife.count;
            DataStorager.SaveMaxLife();
            buySound.Play();
        } else {
            failSound.Play();
        }
    }

    public void ReturnToStart(){
        SceneManager.LoadScene("Start");
    }
}
