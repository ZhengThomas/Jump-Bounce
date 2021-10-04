using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;
    public Transform player;
    [HideInInspector]
    public float maxHeight = 0;

    // Update is called once per frame

    void Update()
    {
        maxHeight = Mathf.Max(player.position.y, maxHeight);
        scoreText.text = (maxHeight/2).ToString("0");
    }
}
