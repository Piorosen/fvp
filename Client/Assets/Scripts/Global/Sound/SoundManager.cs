using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SoundManager : MonoBehaviour {
    [SerializeField]
    public Audio[] Sound;

    private void Start()
    {
        for (int i = 0; i < Sound.Length; i++)
        {
            Sound[i].Initailize(gameObject.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(SoundName.Warrior Name)
    {
        Sound.First(i => i.Name == Name)?.Play();
    }
    public void StopSound(SoundName.Warrior Name)
    {
        Sound.First(i => i.Name == Name)?.Stop();
    }
}
