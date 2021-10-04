using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenFlash : MonoBehaviour
{
    // Start is called before the first frame update
    public float startingAlpha;
    public float timeToFade;
    public Color color;
    public Image[] flashThings;
    float currentAlpha = 0;
    public void flash(float startAlpha, float timeFade, Color clr)
    {
        startingAlpha = startAlpha;
        currentAlpha = startingAlpha;
        color = clr;
        timeToFade = timeFade;

        StopAllCoroutines();
        StartCoroutine("fade");
    }

    IEnumerator fade()
    {
        while(currentAlpha > 0)
        {
            currentAlpha -= (startingAlpha / timeToFade) * Time.deltaTime;
            foreach(Image img in flashThings)
            {
                img.color = new Color(color.r, color.g, color.b, currentAlpha);
            }
            yield return true;
        }
    }
}
