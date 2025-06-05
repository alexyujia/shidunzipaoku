using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FCDisplay : MonoBehaviour
{
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(beatmapManager.GetProgress() >= 1.01f){
            gameObject.GetComponent<TextMeshProUGUI>().text = "ALL PERFECT";
        } else if(beatmapManager.GetCombo() == beatmapManager.GetFullCombo()){
            gameObject.GetComponent<TextMeshProUGUI>().text = "FULL COMBO";
        } else {
            gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
