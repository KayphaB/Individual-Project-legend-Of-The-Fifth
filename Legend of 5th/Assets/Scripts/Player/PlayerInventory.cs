using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public float mycopurite;
    public float keys;
    public float shrombs;

    public TMP_Text mycopuriteText;
    public TMP_Text keysText;
    public TMP_Text shrombsText;

    void Update()
    {
        mycopuriteText.text = "X" + mycopurite.ToString();
        keysText.text = "X" + keys.ToString();
        shrombsText.text = "X" + shrombs.ToString();
    }
}
