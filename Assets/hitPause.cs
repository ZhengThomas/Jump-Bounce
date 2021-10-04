using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitPause : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 0;
    bool canReset = false;

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            if (canReset) Time.timeScale = 1;
        }
    }

    public void startHitPause(float pauseTime)
    {
        timer = pauseTime;
        Time.timeScale = 0;
        canReset = true;
    }
}
