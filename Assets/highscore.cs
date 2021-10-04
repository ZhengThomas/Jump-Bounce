using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class highscore : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI text;
    string saveFolder;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        changeHighscore();
        
    }

    // Update is called once per frame
    public void changeHighscore()
    {
        saveFolder = Application.streamingAssetsPath + "/Saves/";
        string saveText = File.ReadAllText(saveFolder + "/save.txt");
        saveObject save = JsonUtility.FromJson<saveObject>(saveText);

        text.text = save.highScore.ToString("0");
    }
}
