using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAminitus : MonoBehaviour
{
    public int defences;
    public GameObject[] defenceObjects;
    public int state;

    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;
    public float spinSpeed;
    public float degrees;

    private float hitReset;
    private float colorShift;
    public Color hit;
    public Color white;
    private SpriteRenderer sr;

    private float timer;
    public float framesPerSummon;
    public GameObject[] green;
    public GameObject[] blue;
    public GameObject[] red;

    public bool defeated;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        degrees += spinSpeed * Time.deltaTime;
        if (degrees > 360)
        {
            degrees = 0;
        }

        if (!player.GetComponent<PlayerController>().openInventory)
        {
            //turn red for a bit after getting hit
            if (colorShift > 0)
            {
                colorShift -= 1;
                sr.color = hit;
            }
            else
            {
                sr.color = white;
            }

            //decay hitReset over time
            if (hitReset > 0)
            {
                hitReset -= 1;
            }

            //advance summon timer
            timer++;
            if (timer >= framesPerSummon)
            {
                timer = 0;

                for (int i = 0; i < 2; i++)
                {
                    GameObject summon;
                    if (state == 0)
                    {
                        summon = blue[Random.Range(0, blue.Length)];
                    }
                    else if (state == 1)
                    {
                        summon = green[Random.Range(0, green.Length)];
                    }
                    else
                    {
                        summon = red[Random.Range(0, red.Length)];
                    }
                    Instantiate(summon, RandomPos(), Quaternion.identity);
                }
            }
        }

        for (int i = 0; i < defenceObjects.Length; i++)
        {
            defenceObjects[i].SetActive(defences > i);
        }

        anim.SetBool("defeated", defeated);
        anim.SetBool("color", defences < -150);
        anim.SetBool("summon", timer < 25);

        if (defences < 0)
        {
            defences -= 1;
            if (defences / 50 == Mathf.Round(defences))
            {
                Debug.Log("poof");
                Instantiate(poof, RandomPos(), Quaternion.identity);
            }

            if (defences < -300)
            {
                SceneManager.LoadScene("credits");
            }
        }
    }

    private Vector3 RandomPos()
    {
        return new Vector3(
            transform.position.x + Random.Range(-1, 1),
            transform.position.y + Random.Range(-1, 1),
            -5.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //lose a defence spore if hit with a super blast
        if (other.CompareTag("Projectile"))
        {
            if (other.name.Contains("Super"))
            {
                Destroy(other.gameObject);
                defences -= 1;
                state = (state + Random.Range(-1, 2));
                if (state > 2)
                {
                    state = 0;
                }
                else if (state < 0)
                {
                    state = 2;
                }

                if (defences < 0)
                {
                    defeated = true;
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>().d2Beaten = true;
                }
            }
        }
    }
}
