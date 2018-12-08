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

    float RunningTime = 0;

    public void Initailize(AudioSource source)
    {
        Debug.Log(Sound);
        Player = source;
        Player.clip = Sound;
        Player.volume = Volume;
        Player.loop = Loop;
    }

    async void SoundPlay()
    {
        if (!Player.isPlaying)
        {
            Player.Play();
            RunningTime = Time.time;
            await Task.Delay(2500);
            if (Player.isPlaying)
            {
                Player.Stop();
            }
        }
    }


    public void Play()
    {
        SoundPlay();
    }
    public void Stop()
    {
        Debug.Log(Time.time - RunningTime);
        if (Player.isPlaying && Time.time - RunningTime > 2.5f)
        {
            Player.Stop();
        }
    }
}
