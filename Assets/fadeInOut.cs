using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeInOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float fadeInTime;
    public float stayInTime;
    public float fadeOutTime;
    public float maxInAmount;
    public SpriteRenderer sp;

    private float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer < fadeInTime)
        {
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, maxInAmount * (timer / fadeInTime));
        }
        else if (timer < fadeInTime + stayInTime && timer > fadeInTime)
        {

            if(sp.color.a != maxInAmount)
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, maxInAmount);
            }
        }
        else if(timer < fadeOutTime + fadeInTime + stayInTime && timer > fadeInTime + stayInTime)
        {
            float realTime = timer - (fadeInTime + stayInTime);
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, maxInAmount * (1 - (realTime / fadeOutTime)));
        }
        else
        {
            if (sp.color.a != 0)
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0);
            }
        }
    }
}
