using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDunzi_pos : MonoBehaviour
{
    public GameObject fake_dunzi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = fake_dunzi.transform.position - new Vector3(0,1,0);
    }
}
