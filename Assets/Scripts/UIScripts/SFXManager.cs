using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource unitSfx;

    public void PlaySFX(AudioClip Sfx)
    {
        unitSfx.clip = Sfx;
        unitSfx.Play();
    }
}
