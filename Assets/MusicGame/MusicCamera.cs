using UnityEngine;
using System;
public class MusicCamera : MonoBehaviour
{
    public GameObject center;
    private Vector3 offset;
    private float move_timer = 0f;
    private float shake_timer = 1f;
    private bool toShake = false;
    private const float CROSS_TIME = 1f;
    private const float SHAKE_TIME = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - center.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = center.transform.position + offset;
        shakeCamera();
    }

    public void triggerShake()
    {
        if(!DataStorager.settings.notShake){
            toShake = true;
        }
    }

    void shakeCamera()
    {
        if (toShake)
        {
            shake_timer = 0f;
            toShake = false;
        }
        if (shake_timer < SHAKE_TIME)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(shake_timer / SHAKE_TIME - 1, 1 - shake_timer / SHAKE_TIME ), UnityEngine.Random.Range(shake_timer / SHAKE_TIME - 1, 1 - shake_timer / SHAKE_TIME));
        }
        shake_timer += Time.deltaTime;
    }

    Vector3 CalcOffsetByTimer()
    {
        float t = move_timer += Time.deltaTime;
        return offset * (float)(1 - Math.Pow(1 - (t / CROSS_TIME), 4));
    }
}
