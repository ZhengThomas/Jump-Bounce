using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kms : MonoBehaviour
{
    public float dieWhen;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > dieWhen)
        {
            Destroy(gameObject);
        }
    }
}
