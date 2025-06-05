using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameDisplay : MonoBehaviour
{
    public GameObject ghostDunzi;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMesh>().text = ghostDunzi.GetComponent<GhostDunzi>().playerID;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMesh>().text = ghostDunzi.GetComponent<GhostDunzi>().playerID;
    }
}
