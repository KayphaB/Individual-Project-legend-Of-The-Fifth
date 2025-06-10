using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    public float HP = 3;

    public ScreenFade screenFade;
    public GameObject deathScreen;
    private Vector3 checkpoint = new Vector3(0, 0, -1);
    private Vector3 cameraPoint = new Vector3(0, 2.03f, -10);
    private float savedHealth = 3;
    public GameObject camera;
    
    public float immuneFrames;

    public float hitFrames;
    private SpriteRenderer sr;
    public Color white;
    public Color onHit;
    private void Start()
    {
        sr = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //cap health at the max
        if (HP > maxHP)
        {
            HP = maxHP;
        }

        //when dead for 1 second, fade away the screen and show death screen
        deathScreen.SetActive(false);
        if (HP <= -100)
        {
            screenFade.on = true;
            if (screenFade.stage == 4)
            {
                deathScreen.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    HP = savedHealth;
                    transform.parent.transform.position = checkpoint;
                    camera.transform.position = cameraPoint;
                    screenFade.on = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (hitFrames > 0)
        {
            hitFrames -= 1;
            sr.color = onHit;
        }
        else
        {
            sr.color = white;
        }

        if (immuneFrames > 0)
        {
            immuneFrames -= 1;
        }

        if (HP <= 0)
        {
            HP -= 1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && immuneFrames == 0)
        {
            HP -= 0.5f;
            immuneFrames = 40;
            hitFrames = 10;
        }
    }

    public void SetCheckpoint()
    {
        cameraPoint = camera.transform.position;
        checkpoint = transform.position;
        savedHealth = HP;
    }
}
