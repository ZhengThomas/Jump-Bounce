using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playbutton;
    public ballStuff ball;
    public Button[] buttons;
    public TextMeshProUGUI highScore;
    public audioManager audio;
    Image img;
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        audio.PlaySound("click");
        StartCoroutine("gameStart");
        fadeMenu();
        /*
        playbutton.GetComponent<Button>().enabled = false;
        playbutton.GetComponent<Image>().enabled = false;   
        */
    }

    public void fadeMenu()
    {
        StartCoroutine("startMenuFade");
    }
    IEnumerator startMenuFade()
    {
        float speed = 3;
        float maxAlpha = 160f / 255f;
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        //List<Image> images = new List<Image>();
        texts.Add(highScore);
        texts.Add(highScore.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1]);
        texts.Add(GetComponentInChildren<TextMeshProUGUI>());

        List<Image> images = new List<Image>();

        foreach (Button butt in buttons)
        {
            if (butt == null) { continue; }
            if (img.color.a > 0.01f) { butt.interactable = false; }
            else { butt.interactable = true; }
            images.Add(butt.image);
        }

        if (img.color.a > 0.01f)
        {
            while (img.color.a > 0.01f)
            {
                img.color -= new Color(0, 0, 0, Time.unscaledDeltaTime * speed * maxAlpha);
                foreach (Image imag in images) { imag.color -= new Color(0, 0, 0, Time.unscaledDeltaTime * speed); }
                foreach (TextMeshProUGUI txt in texts) { txt.color -= new Color(0, 0, 0, Time.unscaledDeltaTime * speed); }
                yield return null;
            }
            foreach (TextMeshProUGUI txt in texts) { txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0); }
            foreach (Image imag in images) { imag.color = new Color(imag.color.r, imag.color.g, imag.color.b, 0); }
        }
        else
        {
            while (img.color.a < maxAlpha)
            {
                img.color += new Color(0, 0, 0, Time.unscaledDeltaTime * speed);
                foreach (Image imag in images) { imag.color += new Color(0, 0, 0, Time.unscaledDeltaTime * speed); }
                foreach (TextMeshProUGUI txt in texts) { txt.color += new Color(0, 0, 0, Time.unscaledDeltaTime * speed); }
                yield return null;
            }
            foreach (TextMeshProUGUI txt in texts) { txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1); }
            foreach (Image imag in images) { imag.color = new Color(imag.color.r, imag.color.g, imag.color.b, 1); }
            img.color = new Color(img.color.r, img.color.g, img.color.b, maxAlpha);
        }
    }

    IEnumerator gameStart()
    {
        //wait on frame, do stuff here
        yield return null;
        ball.playing = true;

        
        List<Image> images = new List<Image>();
        foreach (Button butt in buttons)
        {
            butt.interactable = false;
            images.Add(butt.image);
        }

        //just in case my code sucks
        int loops = 0;

        while (images[0].color.a > 0.01f && loops < 1000)
        {
            loops += 1;
            yield return null;
        }

        foreach(Button butt in buttons)
        {
            if(butt.gameObject.layer != 10)
            {
                Destroy(butt.gameObject);
            }
        }
    }
}
