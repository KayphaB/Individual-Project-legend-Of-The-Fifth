using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int heartNum;
    public PlayerHealth player;
    
    private Image image;
    private Color alphaChange = new Color(255f, 255f, 255f, 0f);

    public Sprite full;
    public Sprite half;
    public Sprite empty;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        //turn heart invisible if max health is lower than the heart number
        if (player.maxHP < heartNum)
        {
            alphaChange.a = 0;
        }
        else
        {
            alphaChange.a = 255;
        }
        image.color = alphaChange;

        //set the heart sprite to full if appropriate and half-full if appropriate
        if (player.HP - heartNum >= 0)
        {
            image.sprite = full;
        }
        else if (player.HP - heartNum == -0.5f)
        {
            image.sprite = half;
        }
        else
        {
            image.sprite = empty;
        }
    }
}
