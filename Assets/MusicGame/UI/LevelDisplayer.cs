using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayer : MonoBehaviour
{
    public Sprite[] LevelPresents;
    public float level;
    public TMP_Text level_object;
    // Start is called before the first frame update
    void Start()
    {
        level_object.text = level.ToString();
        // 评级
        if(level < 6){
            gameObject.GetComponent<Image>().sprite = LevelPresents[3];
        } else if (level < 10){
            gameObject.GetComponent<Image>().sprite = LevelPresents[2];
        } else if (level < 13){
            gameObject.GetComponent<Image>().sprite = LevelPresents[1];
        } else {
            gameObject.GetComponent<Image>().sprite = LevelPresents[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
