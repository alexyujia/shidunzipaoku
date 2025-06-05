using System;
using System.Collections.Generic;
using UnityEngine;

public class CrashingMove : MonoBehaviour
{
    public List<string> buffTags = new();
    public List<float> buffTimes = new();

    private float move_timer = 0f; // 计时器
    // Star is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // addBuff("invincible",100000);
    }

    public void addBuff(string buffname, float time)
    {
        if (buffTags.Contains(buffname))
        {
            buffTimes[buffTags.IndexOf(buffname)] += time;
        }
        else
        {
            buffTags.Add(buffname);
            buffTimes.Add(time);
        }
    }

    public bool checkBuff(string buffname)
    {
        return buffTags.Contains(buffname);
    }

    void ComsumeBuff()
    {
        for (int i = 0; i < buffTags.Count; i++)
        {
            buffTimes[i] -= Time.deltaTime;
            if (buffTimes[i] < 0)
            {
                buffTimes.RemoveAt(i);
                buffTags.RemoveAt(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ComsumeBuff();
        if(!checkBuff("invincible")){
            Destroy(gameObject);
        }
    }
}
