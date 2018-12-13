using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class Audio {
    public AudioClip Sound;
    public AudioSource Player;
    public bool Loop = false;
    public float Volume = 1.0f;
    public SoundName.Warrior Name;
    

    public void Initailize(AudioSource source)
    {
        Player = source;
        Player.clip = Sound;
        Player.volume = Volume;
        Player.loop = Loop;
    }
    
    public void Play()
    {
        if (!Player.isPlaying)
        {
            Player.Play();
        }
    }
    public void Stop()
    {
        if (Player.isPlaying)
        {
            Player.Stop();
        }
    }
}
