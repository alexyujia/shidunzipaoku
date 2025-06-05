using System;
using Lofelt.NiceVibrations;
using UnityEngine;

public class MusicObstacle : MonoBehaviour
{
    private bool isTouched = false;
    private bool isPerfect = false;
    private bool isInit = true;
    private bool isLast = false;
    public bool isBest = false;
    private float last_frame_pos_z = 0;
    public GameObject player;
    public GameObject camera;
    public GameObject perfectboom;
    public GameObject greatboom;
    public AudioSource bestSound;
    public BeatmapManager beatmapManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GenerateBoom(GameObject theboom){
        camera.GetComponent<MusicCamera>().triggerShake();
        var newboom = Instantiate(theboom);
        newboom.transform.position = gameObject.transform.position + new Vector3(0,2.5f,0);

        if(isBest){
            bestSound.Play();
        }

        if(!DataStorager.settings.notVibrate){
            HapticPatterns.PlayEmphasis(1.0f, 0.0f);
        }

    }

    public void setNote(){
        isInit = false;
    }

     public void setBestNote(){
        isBest = true;
    }

    public void setLastNote(){
        isLast = true;
    }

    void triggerEnd() {
        beatmapManager.triggerEnd();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 2 * (player.GetComponent<Player>().GetVelocity() / 50)
          && Math.Abs(player.transform.position.x - gameObject.transform.position.x) < 1
          && Math.Abs(player.transform.position.y - gameObject.transform.position.y) < 2
        ){
            isTouched = true;
        }
        // 下落墩子也可以
        if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 3 * (player.GetComponent<Player>().GetVelocity() / 50) && player.GetComponent<Player>().isDroping()
            && Math.Abs(player.transform.position.x - gameObject.transform.position.x) < 0.1
            && player.transform.position.y >= gameObject.transform.position.y
        ){
            isTouched = true;
            if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 2 * (player.GetComponent<Player>().GetVelocity() / 50)){
                isPerfect = true;
            };
        }
        if( Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 1.5 * (player.GetComponent<Player>().GetVelocity() / 50)
            && Math.Abs(player.transform.position.x - gameObject.transform.position.x) < 1
            && Math.Abs(player.transform.position.y - gameObject.transform.position.y) < 1.5
        ){
            isPerfect = true;
        }

        // 帧率过低的优化
        if(player.transform.position.z >= gameObject.transform.position.z && player.transform.position.y < 0.1 && gameObject.transform.position.y < 1){
            if(last_frame_pos_z < gameObject.transform.position.z){
                if(Math.Abs(player.transform.position.x - gameObject.transform.position.x) < 0.1){
                    isTouched = true;
                    if(Math.Abs(player.transform.position.z - gameObject.transform.position.z) < 1.5 * (player.GetComponent<Player>().GetVelocity() / 50)){
                        isPerfect = true;
                    };
                }
            }
        }

        if(player.transform.position.z >= gameObject.transform.position.z && isTouched && !isInit){
            if(isPerfect){
                GenerateBoom(perfectboom);
                beatmapManager.AddNowPoint(1);
                if(isBest){
                    beatmapManager.AddNowBest(1);
                }
            } else {
                GenerateBoom(greatboom);
                beatmapManager.AddNowPoint(0.95f);
                if(isBest){
                    beatmapManager.AddNowBest(0.95f);
                }
            }
            if(isLast){
                triggerEnd();
            }
            Destroy(gameObject);
            return;
        }
        if(player.transform.position.z - gameObject.transform.position.z > 10 && !isInit){
            // GenerateBoom();
            beatmapManager.Miss();
            if(isLast){
                triggerEnd();
            }
            Destroy(gameObject);
            return;
        }
        last_frame_pos_z = player.transform.position.z;
    }
}
