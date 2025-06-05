using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingDisplay : MonoBehaviour
{
    public Sprite[] Presents;
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().sprite = Presents[beatmapManager.GetRating()];
    }
}
