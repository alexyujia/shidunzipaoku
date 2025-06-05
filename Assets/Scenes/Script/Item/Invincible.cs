using Unity.Mathematics;
using UnityEngine;

public class Invincible : MonoBehaviour
{

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
            other.GetComponent<Move>().addBuff("invincible", 10f);
            Destroy(gameObject);
        }
    }

    public void HandleTrigger(Collider other){
        OnTriggerEnter(other);
    }
}
