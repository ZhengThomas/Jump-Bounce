using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class soundChanger : MonoBehaviour
{
    Image img;
    public Sprite muted;
    public Sprite unMuted;
    public audioManager audio;
    private void Start()
    {
        img = GetComponent<Image>();

        string saveText = File.ReadAllText(Application.streamingAssetsPath + "/Saves/" + "/save.txt");
        saveObject save = JsonUtility.FromJson<saveObject>(saveText);

        if (save.muted)
        {
            audioManager.globalVolume = 0;
            img.sprite = muted;
        }
    }
    public void muteUnmute()
    {
        if (audioManager.globalVolume == 0)
        {
            img.sprite = unMuted;
            audioManager.globalVolume = 1;
        }
        else if(audioManager.globalVolume == 1)
        {
            img.sprite = muted;
            audioManager.globalVolume = 0;
        }
        audio.PlaySound("click");
    }
}
