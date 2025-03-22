using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // make this singelton
    public static AudioManager Instance;

    public Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optionally, persist this instance across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, destroy this one
            Debug.Log("Destroying duplicate AudioManager instance");
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
        Play("Background");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void SetPitch(string name, float newPitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.pitch = newPitch;
    }

    public void SetVolume(string name, float newVolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.volume = newVolume;
    }
}
