using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLand : MonoBehaviour
{
    public GameObject player;
    bool isInit = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setLand(){
        isInit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.z - gameObject.transform.position.z > 100 && !isInit){
            Destroy(gameObject);
        }
    }
}
