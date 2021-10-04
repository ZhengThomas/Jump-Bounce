using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saviourGroundStuff : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public Transform playerPos;
    public float groundOffset;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < playerPos.transform.position.y - groundOffset)
        {
            transform.position = new Vector2(transform.position.x, playerPos.transform.position.y - groundOffset);
        }
    }

    public void grow(float amount)
    {
        anim.Play("flashFromGrowth");
        transform.localScale += new Vector3(amount,0,0);
    }
    public void shrink(float amount)
    {
        anim.Play("fuckedUp");
        transform.localScale -= new Vector3(amount, 0, 0);
        if (transform.localScale.x <= 0)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }
    }
}
