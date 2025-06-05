using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform camara_trans;
    // Start is called before the first frame update
    void Start()
    {
        camara_trans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(camara_trans);
    }
}
