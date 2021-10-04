using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinGameJuice : MonoBehaviour
{
    // Start is called before the first frame update
    public float spinTime;
    public float dist;
    public GameObject spinObj;
    public GameObject[] spinObjects;

    private bool spin = false;
    private float timer = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            timer += Time.deltaTime;

            if(timer > spinTime)
            {
                spin = false;
            }
            else
            {
                //graph sqrt(1 - x^2) to see what im doing here;
                //float spinAmount = 360 * Mathf.Sqrt(1 - Mathf.Pow(-1 + (timer / spinTime), 2));
                float spinAmount = 360 * (1 - Mathf.Pow(-1 + (timer / spinTime), 2));
                transform.rotation = Quaternion.Euler(0, 0, spinAmount);
            }
        }
        else timer = 0;
    }

    public void Spin(int amount)
    {
        foreach(GameObject obj in spinObjects)
        {
            Destroy(obj);
        }
        timer = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        spinObjects = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            float turnAmount = Mathf.Deg2Rad * (90 + (360 * ((float)i / (float)amount)));
            Vector2 dir = new Vector2(Mathf.Cos(turnAmount), Mathf.Sin(turnAmount));

            Vector3 posToPlace = new Vector3(dir[0] * dist, dir[1] * dist, -10);
            
            spinObjects[i] = Instantiate(spinObj, transform.position, Quaternion.Euler(0,0,(turnAmount * Mathf.Rad2Deg) - 90));
            spinObjects[i].transform.parent = transform;
            spinObjects[i].transform.localPosition = posToPlace;
            
            spin = true;
        }
        
    }


}
