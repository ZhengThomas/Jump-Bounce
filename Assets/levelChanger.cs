using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class levelChanger : MonoBehaviour
{
    string changeTo;
    public Animator anim;
    public void changeLevel(string levelName)
    {
        changeTo = levelName;

        string currentSaveText = File.ReadAllText(deathHandler.saveFolder + "/save.txt");
        saveObject currentSave = JsonUtility.FromJson<saveObject>(currentSaveText);

        if(audioManager.globalVolume == 0) { currentSave.muted = true; }
        else { currentSave.muted = false; }

        string jsonForm = JsonUtility.ToJson(currentSave);
        File.WriteAllText(deathHandler.saveFolder + "/save.txt", jsonForm);

        anim.SetTrigger("fadeOut");

    }
    
    public void changeScene()
    {
        //stuff runs by animation trigger
        SceneManager.LoadScene(changeTo);
    }
}
