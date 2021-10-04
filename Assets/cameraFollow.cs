using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followObj;
    public float heightDiff;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y + heightDiff < followObj.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, followObj.transform.position.y - heightDiff, -10);
        }
    }
}
