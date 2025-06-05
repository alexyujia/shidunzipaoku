using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    public GameObject ComboNum;
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ComboNum.GetComponent<TextMeshProUGUI>().text = beatmapManager.GetCombo().ToString();
    }
}
