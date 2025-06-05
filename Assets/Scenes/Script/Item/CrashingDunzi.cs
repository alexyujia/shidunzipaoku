using Unity.Mathematics;
using UnityEngine;

public class CrashingDunzi : MonoBehaviour
{

    public GameObject crashingDunzi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.activeSelf && other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<AudioSource>().Play();
            GameObject newDunzi = Instantiate(crashingDunzi);
            newDunzi.GetComponent<CrashingMove>().addBuff("invincible",2);
            newDunzi.transform.position = other.transform.position + new Vector3(0,1.25f,-6f);
            newDunzi.GetComponent<Rigidbody>().angularVelocity = new Vector3(other.GetComponent<Move>().GetVelocity().z * 10,0,0);
            newDunzi.GetComponent<Rigidbody>().velocity = new Vector3(0,0,(float)(other.GetComponent<Move>().GetVelocity().z * 4));
            Destroy(gameObject);
        }
    }

    public void HandleTrigger(Collider other){
        OnTriggerEnter(other);
    }
}
