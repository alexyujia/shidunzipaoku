using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject CoinDisplay;
    public GameObject LifeDisplay;
    private string coin_text;
    private string life_text;
    void Start()
    {
        coin_text = CoinDisplay.GetComponent<Text>().text;
        life_text = LifeDisplay.GetComponent<Text>().text;
    }

    // Update is called once per frame
    void Update()
    {
        CoinDisplay.GetComponent<Text>().text = coin_text + DataStorager.coin.count;
        LifeDisplay.GetComponent<Text>().text = life_text + DataStorager.maxLife.count;
    }
}
