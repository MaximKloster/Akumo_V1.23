using UnityEngine;
using System.Collections;

public class BackgroundMusicScript : MonoBehaviour
{
    public AudioClip backgroundMusic;
    AudioSource backgroundMusics;

    // Use this for initialization
    void Start()
    {
        // Play background music in hole game
        DontDestroyOnLoad(gameObject);

        // Set background music
        backgroundMusics = gameObject.AddComponent<AudioSource>();
        backgroundMusics.clip = backgroundMusic;
        // Loop, volume
        backgroundMusics.loop = true;
        backgroundMusics.volume = 0.1f;

        backgroundMusics.Play();
    }

    void Update()
    {
        // Mute sound 
        backgroundMusics.mute = PlayerPrefs.GetInt("Sound_Buttons") == 1 ? true : false;
    }
}
