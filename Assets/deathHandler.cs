using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class deathHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerPos;
    public score scoreStuff;
    public mainMenu menu;
    public float dieWhen;
    bool neverAgain = false;
    public GameObject restartButton;
    public levelChanger levelChange;
    public highscore highscoreText;
    public audioManager audio;
    public static string saveFolder;

    // Update is called once per frame
    private void Start()
    {
        saveFolder = Application.streamingAssetsPath + "/Saves/";
    }
    void Update()
    {
        if((playerPos.position.y < transform.position.y - dieWhen) && !neverAgain)
        {
            neverAgain = true;
            youDied(scoreStuff.maxHeight / 2);
        }
    }

    void youDied(float score)
    {
        //playerPos.transform.position += new Vector3(0, 100, 0);

        string jsonFile = File.ReadAllText(saveFolder + "/save.txt");
        saveObject loadedSave = JsonUtility.FromJson<saveObject>(jsonFile);
        print("saving...");
        if (score > loadedSave.highScore)
        {
            saveObject save = new saveObject();
            save.highScore = score;

            string jsonForm = JsonUtility.ToJson(save);
            File.WriteAllText(saveFolder + "/save.txt", jsonForm);
            print("saved new highscore");
            highscoreText.changeHighscore();
        }

        StartCoroutine("fadeInButton");
        menu.fadeMenu();
        playerPos.gameObject.GetComponent<ballStuff>().playing = false;
        Time.timeScale = 0;
    }

    IEnumerator fadeInButton()
    {
        var button = restartButton.GetComponent<Button>();
        var img = restartButton.GetComponent<Image>();
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        texts.Add(restartButton.GetComponentInChildren<TextMeshProUGUI>());

        button.enabled = true;
        img.enabled = true;
        foreach (TextMeshProUGUI text in texts) { text.enabled = true; }

        //time to fade in is 1/speed seconds
        float speed = 12;
        while (img.color.a < 0.99f)
        {
            img.color += new Color(0, 0, 0, Time.unscaledDeltaTime * speed);
            foreach (TextMeshProUGUI text in texts) { text.color += new Color(0, 0, 0, Time.unscaledDeltaTime * speed); }
            yield return null;
        }
    }

    public void restart()
    {
        audio.PlaySound("click");
        levelChange.changeLevel("SampleScene");
    }
}
