using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBouncies : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D playerBall;
    public float spawnBeneathMin;
    public float spawnBeneathMax;
    public float spawnSpeed;
    private float timer = -1;
    public GameObject enemy;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playerBall.velocity.y >= -10.5f)
        {
            timer += Time.deltaTime;
            if(timer >= spawnSpeed)
            {
                timer -= spawnSpeed * Random.Range(0.9f,1.1f);
                spawnEnemy();
            }
        }
    }

    void spawnEnemy()
    {
        float enemyDir = 1;
        if (Random.value < 0.5f) enemyDir = -1;

        Vector2 spawnPos = new Vector3(enemyDir * 15, transform.position.y - Random.Range(spawnBeneathMin, spawnBeneathMax + 1) + Mathf.Clamp(playerBall.velocity.y / 3f, 0, 100));

        var spawnedEnemy = Instantiate(enemy, spawnPos, transform.rotation);
    }
}
