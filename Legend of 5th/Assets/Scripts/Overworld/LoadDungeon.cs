using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour
{
    public ScreenFade screenFade;
    private bool duplicate;
    public string dungeonScene;

    public GameObject camera;
    public GameObject UI;
    void Start()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the cave, then teleport the player to the set destination
        if (other.name == "Player" && !duplicate)
        {
            duplicate = true;
            StartCoroutine(CaveEnter(other));
        }
    }

    IEnumerator CaveEnter(Collider2D player)
    {
        //freeze player movement and wait until screen fades to black
        player.GetComponent<PlayerController>().enteringCave = true;
        screenFade.timer = 0;
        screenFade.on = true;
        yield return new WaitForSeconds(screenFade.delay / 50 * 4f);

        DontDestroyOnLoad(player);
        player.transform.position = new Vector3(0, 0, 0);
        DontDestroyOnLoad(camera);
        DontDestroyOnLoad(UI);

        SceneManager.LoadScene(dungeonScene);
    }
}
