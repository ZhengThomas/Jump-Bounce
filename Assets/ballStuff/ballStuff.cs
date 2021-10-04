using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballStuff : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float smashSpeed;
    public float bouncePower;
    private Vector2 previousPos;
    public LayerMask groundLayer;
    public float size;
    public saviourGroundStuff saviourGround;
    private float impactTime;
    private float clickTime;
    private float existenceTimer = 0;
    public LineRenderer line;
    public Vector2 velocity;
    public float maxSmashPower;

    public audioManager soundMaker;

    private string moveState;
    public float camWidth;

    private float smashUses = 1;
    private float consecutivePerfects = 0;
    private float timePerfect = 0.05f;
    private float timePerfectEdited = 0;
    private float startSmashCD = 0;
    private float canPerfectCD = 0;
    private bool hitGroundRecent = false;
    private bool hitEnemyRecent = false;
    private bool hitPerfect = false;

    private Animator anim;
    public spinGameJuice spinThing;
    public hitPause timePause;
    public screenFlash flashy;
    public ParticleSystem dashParticles;

    [HideInInspector]
    public int tutorialState = 1;
    public Animator mouseClickAnim;
    private float timeInTutorial = 0;

    [HideInInspector]
    public bool playing = false;



   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPos = transform.position;
        moveState = "fall";
        impactTime = 0;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Time.fixedDeltaTime = Time.timeScale * 0.015f;
        startSmashCD -= Time.deltaTime;
        canPerfectCD -= Time.deltaTime;

        if (!playing)
        {
            Time.timeScale = 0;
            return;
        }
        print(tutorialState);
        switch (tutorialState)
        {
            case 1:
                Time.timeScale = 0;
                timeInTutorial += Time.unscaledDeltaTime;
                if(timeInTutorial > 1)
                {
                    mouseClickAnim.Play("fadeIn");
                    timeInTutorial = -10000;
                }
                break;
            case 2:
                if (timeInTutorial < 0)
                {
                    mouseClickAnim.Play("fadeOut");
                }
                Time.timeScale = 0;
                timeInTutorial = 0;
                break;
            case 3:
                timeInTutorial = 0;
                break;
            case 4:
                Time.timeScale = 0;
                timeInTutorial += Time.unscaledDeltaTime;
                if (timeInTutorial > 1)
                {
                    mouseClickAnim.Play("fadeIn");
                    timeInTutorial = -10000;
                }
                break;
            case 5:
                if (timeInTutorial < 0)
                {
                    mouseClickAnim.Play("fadeOut");
                    timeInTutorial = 0;
                    tutorialState += 1;
                }
                break;
        }



        //feels bad for downards drag so only upwards
        if (rb.velocity.y > 0)
        {
            rb.drag = Mathf.Clamp(rb.velocity.y / 10, 2f, 10);
        }
        else 
        {
            rb.drag = 0;
        }
        
        existenceTimer += Time.deltaTime;

        //gravity goes from low to normal at peak of jump arc
        rb.gravityScale = Mathf.Clamp(Mathf.Abs(rb.velocity.y / 12), 0.4f, 1);

        //since unity sucks ass with coillision and fast moving objects, this makes sure that you cant move through objects by
        //raycawsting where you were last frame with where you were now.
        var groundCheck = Physics2D.Raycast(previousPos, (Vector2)transform.position - previousPos, Vector2.Distance(previousPos, transform.position), groundLayer);
        if (groundCheck.collider != null)
        {
            transform.position = groundCheck.point;
            Vector2 movementDir = Vector3.Normalize(rb.velocity);
            transform.position -= new Vector3(movementDir.x * size, movementDir.y * size);
            bounce();
        }

        previousPos = new Vector2(transform.position.x, transform.position.y);


        //if any shit is nearby then we move to impact and can no longer dash
        //may feel wrong if you let yourself fall, get close to an enemy, pass it, and then try to dash,
        //but i am not going to program that
        var nearbyCheck = Physics2D.OverlapCircle(transform.position, size + Mathf.Abs((rb.velocity.y + rb.velocity.x) / 40) + 0.5f, groundLayer);
        //var slowDownCheck = Physics2D.CircleCast(transform.position, size, Vector3.Normalize(rb.velocity), Mathf.Abs((rb.velocity.y  + rb.velocity.x) / 30), groundLayer);
        if (nearbyCheck != null && rb.velocity.y <= 0)
        {
            moveState = "impact";
        }
        else
        {
            if (rb.velocity.y >= 3 && startSmashCD <= 0)
            { 
                moveState = "fall";
                hitPerfect = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //clicks can only happen once in a while to pervent spamming being viable
            //as in for clicking on perfect timing
            if(canPerfectCD <= 0)
            {
                clickTime = existenceTimer;
                canPerfectCD = timePerfect * 5;
            }
            if (moveState == "impact")
            {
                if (Mathf.Abs((existenceTimer - impactTime)) <= timePerfectEdited && !hitPerfect)
                {
                    perfectTiming();
                }
            }

            if (tutorialState < 2)
            {
                tutorialState += 1;
            }
        }

        //idk why i get these every frame but it breaks if i put it inside the holding part
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 flightDir = Vector3.Normalize(mousePos - (Vector2)transform.position);

        line.enabled = false;
        if(flightDir.y > 0)
        {
            flightDir = Vector3.Normalize(new Vector3(flightDir.x, 0));
        }

        //holding down dash to show the line
        if (Input.GetMouseButton(0) && moveState == "fall" && smashUses >= 1)
        {
            if(tutorialState > 2) { Time.timeScale = 0.7f; }
             
            line.enabled = true;

            /*
            Vector2 futureVel = flightDir * smashSpeed;

            Vector2[] angryBirdsLine = Plot(rb, (Vector2)transform.position, futureVel, 500);
            line.positionCount = angryBirdsLine.Length;

            Vector3[] positions = new Vector3[angryBirdsLine.Length];
            for (int i = 0; i < angryBirdsLine.Length; i++) { positions[i] = angryBirdsLine[i]; }
            */

            Vector3[] linePositions = new Vector3[2];
            linePositions[0] = transform.position;

            float dist = Vector2.Distance(transform.position, mousePos);
            float powerVall = Mathf.Clamp(smashSpeed * dist, 4, maxSmashPower);
            powerVall = maxSmashPower; //just forced to max cuz im gay

            Vector3 futurePoint = transform.position + ((Vector3)flightDir * powerVall);
            linePositions[1] = futurePoint;

            line.positionCount = linePositions.Length;
            line.SetPositions(linePositions);
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            //this is the dash stuff
            if (moveState == "fall" && smashUses >= 1)
            {
                float dist = Vector2.Distance(transform.position, mousePos);
                float powerVal = Mathf.Clamp(smashSpeed * dist, 4, maxSmashPower);
                powerVal = maxSmashPower;

                rb.velocity = flightDir * powerVal;
                smashUses -= 1;
                if (flightDir.x > 0) soundMaker.PlaySound("dashSound");
                else soundMaker.PlaySound("dashSound2");
                
                Time.timeScale = 1;
                if (tutorialState < 10)
                {
                    tutorialState += 1;
                }

                
                var dirShot = mousePos - (Vector2)transform.position;
                dirShot = Vector3.Normalize(dirShot);
                var angle = Mathf.Atan2(dirShot.x, dirShot.y) * Mathf.Rad2Deg;
                var fuck = Instantiate(dashParticles, (Vector2)transform.position + (dirShot*2), Quaternion.Euler(angle + 90,90,0));
                fuck.Play();
            }
        }

        //for when you fail a perfect timing off an enemy or ground
        //poterntially more functionality for when you do perfect time but you want something to happen when you coulda failed
        //but for now its gonna be like this
        if(hitEnemyRecent && existenceTimer - impactTime > timePerfectEdited)
        {
            hitEnemyRecent = false;
            if (!hitPerfect)
            {
                consecutivePerfects = 0;
                soundMaker.PlaySound("failedPing");
            }            
        }
        if (hitGroundRecent && existenceTimer - impactTime > timePerfectEdited)
        {
            
            hitGroundRecent = false;
            if (!hitPerfect)
            {
                consecutivePerfects = 0;
                saviourGround.shrink(4);
                flashy.flash(0.5f, 0.5f, new Color(1, 0, 0));
                soundMaker.PlaySound("failedPing");
            }
        }

        //bounce off sides
        if(transform.position.x + size> camWidth)
        {
            transform.position = new Vector2(camWidth - size, transform.position.y);
        }
        if (transform.position.x - size < -camWidth)
        {
            transform.position = new Vector2(-camWidth + size, transform.position.y);
        }

        if (Mathf.Abs(transform.position.x) + size >= camWidth)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            //(transform.position.x / Mathf.Abs(transform.position.x)) << this part means if you are positive mult by pos 1 and if neg mult by neg 1
            
        }

    }
    private void bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, bouncePower / 2);
        smashUses = 1;
        startSmashCD = 0.5f;
        moveState = "impact";
        if (tutorialState <= 3)
        {
            tutorialState += 1;
            var groundWhere = Physics2D.Raycast(transform.position, new Vector2(0, -1), 10, groundLayer);
            transform.position = new Vector3(groundWhere.point.x, groundWhere.point.y + size, 0);
            Time.timeScale = 0;
        }
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        timePerfectEdited = timePerfect;

        if (collision.gameObject.layer == 9)
        {
            var enemyScript = collision.gameObject.GetComponent<enemyBasic>();
            enemyScript.die();
            hitEnemyRecent = true;
        }
        else if(collision.gameObject.layer == 8)
        {
            hitGroundRecent = true;
            //i wanted it to be harder to time on bottom ground but not anymore
            timePerfectEdited = timePerfect / 1;
        }

        if (hitPerfect)
        {
            perfectTiming();
            return;
        }

        bounce();
        impactTime = existenceTimer;

        if (Mathf.Abs((existenceTimer - clickTime) / Time.timeScale) <= timePerfectEdited && !hitPerfect)
        {
            perfectTiming();
        }
    }

    private void perfectTiming()
    {
        rb.velocity = new Vector2(rb.velocity.x, bouncePower);
        consecutivePerfects += 1;
        hitPerfect = true;

        soundMaker.PlaySound("ping" + (consecutivePerfects).ToString());
        spinThing.Spin((int)consecutivePerfects);
        anim.Play("flash");
        //how useful is 0.03 seconds of hitpause? i dont know but i apparently should do it
        timePause.startHitPause(0.03f);

        if (consecutivePerfects >= 8)
        {
            consecutivePerfects = 7;
            saviourGround.grow(0.5f);
            flashy.flash(0.3f, 0.3f, new Color(1, 1, 1));
        }

        if (tutorialState < 10)
        {
            tutorialState += 1;
        }
    }
}
