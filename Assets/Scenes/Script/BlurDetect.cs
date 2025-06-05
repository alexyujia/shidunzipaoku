using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurDetect : MonoBehaviour
{
    void Awake()
    {
        if(!DataStorager.settings.hasMotionBlur){
            gameObject.SetActive(false);
        }
    }
}
