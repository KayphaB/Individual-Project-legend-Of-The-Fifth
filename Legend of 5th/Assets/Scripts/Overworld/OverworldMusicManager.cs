using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OverworldMusicManager : MonoBehaviour
{
    public Transform player;
    public float fadeSpeed = 1.0f;
    public float crossfadeX = 0f;
    public float yThreshold = -5f;

    public AudioSource musicA; // Left side music
    public AudioSource musicB; // Right side music
    public AudioSource priorityMusic; // Y-priority music
    public AudioSource bossMusic; //music plays during boss fight

    public InventoryManager inven;
    public ClassicFollow camera;

    private void Start()
    {
        //determine if the bossfight is currently active
        if (camera.roomID == 28 && inven.itemsUnlocked[3] == 1)
        {
            musicA.volume = 0f;
            musicB.volume = 0f;
            priorityMusic.volume = 0f;
            bossMusic.volume = 1f;
        }
        else 
        {
            // Determine if player starts below yThreshold
            if (player.position.y < yThreshold)
            {
                musicA.volume = 0f;
                musicB.volume = 0f;
                priorityMusic.volume = 1f;
                bossMusic.volume = 0f;
            }
            else
            {
                float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);
                musicA.volume = 1f - t;
                musicB.volume = t;
                priorityMusic.volume = 0f;
                bossMusic.volume = 0f;
            }
        }

        // Start playing all tracks (they're looped, muted if needed)
        musicA.loop = true;
        musicB.loop = true;
        priorityMusic.loop = true;
        bossMusic.loop = true;

        musicA.Play();
        musicB.Play();
        priorityMusic.Play();
        bossMusic.Play();
    }

    private void Update()
    {
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        Debug.Log(camera.roomID + ", " + inven.itemsUnlocked[3]);
        if (camera.roomID == 28 && inven.itemsUnlocked[3] == 1)
        {
            // Fade in boss music, fade out others
            priorityMusic.volume = Mathf.MoveTowards(priorityMusic.volume, 0f, fadeSpeed * Time.deltaTime);
            bossMusic.volume = Mathf.MoveTowards(bossMusic.volume, 1f, fadeSpeed * Time.deltaTime);
            musicA.volume = Mathf.MoveTowards(musicA.volume, 0f, fadeSpeed * Time.deltaTime);
            musicB.volume = Mathf.MoveTowards(musicB.volume, 0f, fadeSpeed * Time.deltaTime);
        }
        else if (player.position.y < yThreshold)
        {
            // Fade in priority music, fade out others
            priorityMusic.volume = Mathf.MoveTowards(priorityMusic.volume, 1f, fadeSpeed * Time.deltaTime);
            bossMusic.volume = Mathf.MoveTowards(bossMusic.volume, 0f, fadeSpeed * Time.deltaTime);
            musicA.volume = Mathf.MoveTowards(musicA.volume, 0f, fadeSpeed * Time.deltaTime);
            musicB.volume = Mathf.MoveTowards(musicB.volume, 0f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Resume X-based crossfade
            float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);
            musicA.volume = Mathf.MoveTowards(musicA.volume, 1f - t, fadeSpeed * Time.deltaTime);
            musicB.volume = Mathf.MoveTowards(musicB.volume, t, fadeSpeed * Time.deltaTime);
            priorityMusic.volume = Mathf.MoveTowards(priorityMusic.volume, 0f, fadeSpeed * Time.deltaTime);
            bossMusic.volume = Mathf.MoveTowards(bossMusic.volume, 0f, fadeSpeed * Time.deltaTime);
        }
    }
}
