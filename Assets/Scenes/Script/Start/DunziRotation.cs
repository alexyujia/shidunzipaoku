using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunziRotation : MonoBehaviour
{
    private float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * 100;
        gameObject.transform.rotation = Quaternion.Euler(0,angle,0);
    }
}
