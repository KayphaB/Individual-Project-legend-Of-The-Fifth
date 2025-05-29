using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Sprite[] sprites;
    public float delay;
    public float timer;
    private int stage;
    public bool on;
    private Image image;
    private Color alphaChange;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void FixedUpdate()
    {
        timer += 1;
        if (timer >= delay)
        {
            timer = 0;
            if (on && stage != 4)
            {
                stage += 1;
            }
            else if (!on && stage != 0)
            {
                stage += -1;
            }
        }

        image.sprite = sprites[stage];

        if (stage == 0)
        {
            alphaChange.a = 0;
        }
        else
        {
            alphaChange.a = 255;
        }
        image.color = alphaChange;
    }
}