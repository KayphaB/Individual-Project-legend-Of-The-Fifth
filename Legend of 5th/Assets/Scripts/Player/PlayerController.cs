using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject shromb;

    private Animator anim;
    public int direction;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        frozen = enteringCave || usingWeapon || cameraTransition || openInventory;

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
            if (GetComponent<ChainController>().length == 0)
            {
                anim.speed = 0;
            }
        }

        //update the animation's variables
        anim.SetInteger("direction", direction);
        anim.SetBool("attacking", GetComponent<ChainController>().length != 0);

        //managing opening and closing inventory
        if (Input.GetKeyDown(KeyCode.Return))
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
                    UI.anchoredPosition.y - inventorySpeed);
            }
        }
        else if (UI.anchoredPosition.y < 0)
        {
            UI.anchoredPosition = new Vector3(
                UI.anchoredPosition.x,
                UI.anchoredPosition.y + inventorySpeed);
        }
        else
        {
            openInventory = false;
        }

        //What happens when you press K
        if (!openInventory && Input.GetKeyDown(KeyCode.K))
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
        }
    }
}
