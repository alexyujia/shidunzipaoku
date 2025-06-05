using UnityEngine;

public class OnHit : MonoBehaviour
{
    // public ParticleSystem boom;
    // public Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBoom(){
        var camera = GlobalTargetManager.camera;
        camera.GetComponent<FixedCamera>().triggerShake();
        var boom = GlobalTargetManager.boom;
        var newboom = Instantiate(boom);
        newboom.transform.position = gameObject.transform.position + new Vector3(0,2.25f,5);
        // newboom.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.activeSelf && other.CompareTag("Player")){
            if(other.GetComponent<Move>().checkBuff("invincible")){
                GenerateBoom();
                Destroy(gameObject);
            } else {
                if(other.GetComponent<Move>().ConsumeLife()){
                    GenerateBoom();
                    Destroy(gameObject);
                } else {
                    other.GetComponent<Move>().KillSelf();
                }
            }
        }
        if (other.CompareTag("CrashingDunzi")){
            GenerateBoom();
            Destroy(gameObject);
        }
    }

    public void HandleTrigger(Collider other){
        OnTriggerEnter(other);
    }
}
