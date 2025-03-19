using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // make this singelton
    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Optionally, persist this instance across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }

    void Start(){
        //Play("Background");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }
}
