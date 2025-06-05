using Unity.Mathematics;
using UnityEngine;

public class SpeedUp : MonoBehaviour
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
            other.GetComponent<Move>().SpeedUp(0.5f);
            Destroy(gameObject);
        }
    }
}
