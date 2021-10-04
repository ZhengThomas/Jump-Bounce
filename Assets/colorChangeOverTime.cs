using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChangeOverTime : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sp;
    public float changeDist;
    public Color[] colors;
    public score scoreStuff;
    public Gradient entireGradient;
    GradientColorKey[] gradiColors;
    GradientAlphaKey[] alphas;

    Color colorNow;
    Color colorLater;
    float whenChange;
    void Start()
    {
        colorNow = colors[Random.Range(0,7)];
        colorLater = colors[Random.Range(0, 7)];

        gradiColors = new GradientColorKey[2];

        gradiColors[0].time = 0;
        gradiColors[1].time = 1;
        alphas = new GradientAlphaKey[1];

        alphas[0].alpha = 1;
        alphas[0].time = 1;

        switchColors();



        whenChange = changeDist;
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreStuff.maxHeight > whenChange)
        {
            whenChange += changeDist;
            switchColors();
        }

        sp.color = entireGradient.Evaluate(((scoreStuff.maxHeight + changeDist - whenChange) / changeDist));
    }
    void switchColors()
    {
        colorNow = colorLater;
        for (int i = 0; i < 15; i++)
        {
            var currColor = colors[Random.Range(0, 7)];
            if(currColor != colorNow)
            {
                colorLater = currColor;
                break;
            }
        }
        gradiColors[0].color = colorNow;
        gradiColors[1].color = colorLater;
        entireGradient.SetKeys(gradiColors, alphas);
    }
}
