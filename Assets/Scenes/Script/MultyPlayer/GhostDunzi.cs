using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GhostDunzi : NetworkBehaviour
{
    [SyncVar]
    public string playerID = "无名墩子";

    [SyncVar]
    private bool alive = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // 叉叉
        gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
        if (isLocalPlayer)
        {
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            gameObject.transform.position = GlobalTargetManager.dunzi.transform.position;
            gameObject.transform.GetChild(1).rotation = GlobalTargetManager.dunzi.transform.rotation;
            alive = GlobalTargetManager.dunzi.GetComponent<Move>().isAlive();
        } else {
            if(!alive){
                foreach (Renderer renderer in gameObject.transform.GetChild(2).GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            } else {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
