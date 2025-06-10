using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool frozen;
    public bool enteringCave;
    public bool usingWeapon;
    public bool cameraTransition;
    public bool openInventory;

    public InventoryManager inven;
    private bool trueOpenInventory;
    public RectTransform UI;
    public float inventorySpeed;

    public UnlockablesManager unlock;

    public GameObject shromb;
    public GameObject dust;
    public GameObject projectile;
    public GameObject projectileTwo;
    private float projectileCooldown;

    private Animator anim;
    public int direction;

    public GameObject woodRaft;
    public GameObject water;
    public GameObject rockRaft;
    public GameObject lava;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        frozen = enteringCave || usingWeapon || cameraTransition || openInventory || transform.GetChild(0).GetComponent<PlayerHealth>().HP <= 0;

        anim.speed = 1;
        if (Input.GetKey(KeyCode.A) && !frozen)
        {
            transform.Translate(transform.right * Time.deltaTime * -speed);
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.D) && !frozen)
        {
            transform.Translate(transform.right * Time.deltaTime * speed);
            direction = 2;
        }
        else if (Input.GetKey(KeyCode.W) && !frozen)
        {
            transform.Translate(transform.up * Time.deltaTime * speed);
            direction = 3;
        }
        else if (Input.GetKey(KeyCode.S) && !frozen)
        {
            transform.Translate(transform.up * Time.deltaTime * -speed);
            direction = 4;
        }
        else
        {
            //if no walking keys pressed and not attacking, stop animation
            if (GetComponent<ChainController>().length == 0 && !(transform.GetChild(0).GetComponent<PlayerHealth>().HP <= 0))
            {
                anim.speed = 0;
            }
        }

        //update the animation's variables
        anim.SetInteger("direction", direction);
        anim.SetBool("attacking", GetComponent<ChainController>().length != 0);
        anim.SetBool("IsDead", transform.GetChild(0).GetComponent<PlayerHealth>().HP <= 0);

        //managing opening and closing inventory
        if (Input.GetKeyDown(KeyCode.Return) && !(transform.GetChild(0).GetComponent<PlayerHealth>().HP <= 0))
        {
            trueOpenInventory = !trueOpenInventory;
        }

        if (trueOpenInventory)
        {
            openInventory = true;
            if (UI.anchoredPosition.y > -820)
            {
                UI.anchoredPosition = new Vector3(
                    UI.anchoredPosition.x,
                    UI.anchoredPosition.y - inventorySpeed * Time.deltaTime);
            }
        }
        else if (UI.anchoredPosition.y < 0)
        {
            UI.anchoredPosition = new Vector3(
                UI.anchoredPosition.x,
                UI.anchoredPosition.y + inventorySpeed * Time.deltaTime);
        }
        else
        {
            openInventory = false;
        }

        //What happens when you press K
        if (!openInventory && Input.GetKeyDown(KeyCode.K) && !frozen)
        {
            if (inven.selected == 0 && GetComponent<PlayerInventory>().shrombs > 0)
            {
                GetComponent<PlayerInventory>().shrombs--;
                float offsetX = 0;
                float offsetY = 0;
                if (direction == 1)
                {
                    offsetX = -0.8f;
                }
                else if (direction == 2)
                {
                    offsetX = 0.8f;
                }
                else if (direction == 3)
                {
                    offsetY = 0.8f;
                }
                else
                {
                    offsetY = -0.8f;
                }
                Vector3 spawnPos = new Vector3(
                        transform.position.x + offsetX,
                        transform.position.y + offsetY,
                        -0.5f);
                Instantiate(shromb, spawnPos, Quaternion.identity);
            }
            else if (inven.selected == 1 && (GetComponent<PlayerInventory>().mycopurite > 0 || inven.itemsUnlocked[1] == 2))
            {
                if (inven.itemsUnlocked[1] == 1)
                {
                    GetComponent<PlayerInventory>().mycopurite--;
                }
                float offsetX = 0;
                float offsetY = 0;
                if (direction == 1)
                {
                    offsetX = -0.8f;
                }
                else if (direction == 2)
                {
                    offsetX = 0.8f;
                }
                else if (direction == 3)
                {
                    offsetY = 0.8f;
                }
                else
                {
                    offsetY = -0.8f;
                }
                Vector3 spawnPos = new Vector3(
                        transform.position.x + offsetX,
                        transform.position.y + offsetY,
                        -0.5f);
                Instantiate(dust, spawnPos, Quaternion.identity);
            }
            else if (inven.selected == 2 && inven.itemsUnlocked[2] == 2)
            {
                inven.itemsUnlocked[2] = 1;
                transform.GetChild(0).GetComponent<PlayerHealth>().HP = transform.GetChild(0).GetComponent<PlayerHealth>().maxHP;
            }
            else if (inven.selected == 4 && GetComponent<PlayerInventory>().mycopurite > 0 && projectileCooldown == 0)
            {
                GetComponent<PlayerInventory>().mycopurite--;
                float offsetX = 0;
                float offsetY = 0;
                if (direction == 1)
                {
                    offsetX = -0.8f;
                }
                else if (direction == 2)
                {
                    offsetX = 0.8f;
                }
                else if (direction == 3)
                {
                    offsetY = 0.8f;
                }
                else
                {
                    offsetY = -0.8f;
                }
                Vector3 spawnPos = new Vector3(
                        transform.position.x + offsetX,
                        transform.position.y + offsetY,
                        -0.5f);

                if (inven.itemsUnlocked[4] == 2)
                {
                    GameObject proj = Instantiate(projectileTwo, spawnPos, Quaternion.identity);
                    proj.GetComponent<Projectile>().direction = direction;
                    projectileCooldown = 40;
                }
                else
                {
                    GameObject proj = Instantiate(projectile, spawnPos, Quaternion.identity);
                    proj.GetComponent<Projectile>().direction = direction;
                    projectileCooldown = 40;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (projectileCooldown > 0)
        {
            projectileCooldown--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == water.GetComponent<TilemapCollider2D>() && unlock.unlockables[3])
        {
            woodRaft.SetActive(true);
        }

        if (other == lava.GetComponent<TilemapCollider2D>() && unlock.unlockables[4])
        {
            rockRaft.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other ==  water.GetComponent<TilemapCollider2D>())
        {
            woodRaft.SetActive(false);
        }

        if (other == lava.GetComponent<TilemapCollider2D>())
        {
            rockRaft.SetActive(false);
        }
    }
}
