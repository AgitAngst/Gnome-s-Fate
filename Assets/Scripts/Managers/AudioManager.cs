using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioClip[] musicClips;
    public AudioSource gnomeStrafe;
    public AudioClip[] gnomeStrafeClips; //0
    public AudioSource gnomePunch;
    public AudioClip[] gnomePunchClips;//1
    public AudioSource gnomeCrystalHit;
    public AudioClip[] gnomeCrystalHitClips;//2
    public AudioSource gnomeDeath;
    public AudioClip[] gnomeDeathClips;//3
    public AudioSource gnomeRunning;
    public AudioClip[] gnomeRunningClip;//4
    public AudioSource gnomeHurt;
    public AudioClip[] gnomeHurtClips;//5
    public AudioSource crystalSmash;
    public AudioClip[] crystalSmashClips;//6
    public AudioSource clientJump;
    public AudioClip[] clientJumpClips;//7



    // public void PlaySound(int index)
    // {
    //     int count = 1;
    //     while (count-- > 0)
    //     {
    //        // gnomeDeath[index].PlayOneShotSoundManaged(gnomeDeath[index].clip);
    //     }
    // }
    private void Start()
    {
        PlayMusic();
    }

    public void PlaySound(int audioSourceIndex)
    {
        switch (audioSourceIndex)
        {
            case 0:
                gnomeStrafe.PlayOneShot(gnomeStrafeClips[Random.Range(0,gnomeStrafeClips.Length)]);
                break;
            case 1:
                gnomePunch.PlayOneShot(gnomePunchClips[Random.Range(0,gnomePunchClips.Length)]);
                break;
            case 2:
                gnomeCrystalHit.PlayOneShot(gnomeCrystalHitClips[Random.Range(0,gnomeCrystalHitClips.Length)]);
                break;
            case 3:
                gnomeDeath.PlayOneShot(gnomeDeathClips[Random.Range(0,gnomeDeathClips.Length)]);
                break;
            case 4:
                gnomeRunning.PlayOneShot(gnomeRunningClip[Random.Range(0,gnomeRunningClip.Length)]);
                break;
            case 5:
                gnomeHurt.PlayOneShot(gnomeHurtClips[Random.Range(0,gnomeHurtClips.Length)]);
                break;
            case 6:
                crystalSmash.PlayOneShot(crystalSmashClips[Random.Range(0,crystalSmashClips.Length)]);
                break;
            case 7:
                clientJump.PlayOneShot(clientJumpClips[Random.Range(0,clientJumpClips.Length)]);
                break;
            
        }
    }

    public void PlayMusic()
    {
        music.clip = musicClips[Random.Range(0, musicClips.Length)];
        music.Play();
    }
}