using System.Collections;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    public static MusicBox instance;
    public AudioClip shootSound;
    public AudioClip coolSound;
    public AudioSource source;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playShootSound()
    {
        source.PlayOneShot(shootSound, 1);
    }

    public void playCoolSound()
    {
        source.PlayOneShot(coolSound, 1);
    }
}