using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDunzi_rot : MonoBehaviour
{
    public GameObject fake_dunzi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = fake_dunzi.transform.rotation;
    }
}
