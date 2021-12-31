using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;

public class AudioManager : MonoBehaviour
{
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
    public AudioSource clientJump;
    public AudioClip[] clientJumpClips;//5



    // public void PlaySound(int index)
    // {
    //     int count = 1;
    //     while (count-- > 0)
    //     {
    //        // gnomeDeath[index].PlayOneShotSoundManaged(gnomeDeath[index].clip);
    //     }
    // }

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
                clientJump.PlayOneShot(clientJumpClips[Random.Range(0,clientJumpClips.Length)]);
                break;
            
        }
    }
}