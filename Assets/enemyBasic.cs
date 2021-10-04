using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBasic : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public GameObject deathParticles;
    Vector2 direction;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector3.Normalize(new Vector2(-transform.position.x, 0));
        rb.velocity = direction * speed * (1 + Mathf.Min(transform.position.y / 350, 2f));
        
    }

    // Update is called once per frame
    void Update()
    {
        //im just guessing 20 is offscreen
        if (Mathf.Abs(transform.position.x) > 20)
        {
            Destroy(gameObject);
        }
    }

    public void die()
    {
        Destroy(gameObject);
        Instantiate(deathParticles, transform.position, transform.rotation);
    }
}
