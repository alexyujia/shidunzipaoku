using TMPro;
using UnityEngine;

public class ProgressText : MonoBehaviour
{
    public BeatmapManager beatmapManager;
    public TMP_ColorGradient[] colorPresents;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float proress = beatmapManager.GetProgress();
        if(proress < 0.6){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[4];
        }
        else if(proress < 0.8){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[3];
        }
        else if(proress < 0.97){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[2];
        }
        else if(proress < 1){
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[1];
        } else {
            gameObject.GetComponent<TextMeshProUGUI>().colorGradientPreset = colorPresents[0];
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = (beatmapManager.GetProgress() * 100).ToString("0.0000");
    }
}
