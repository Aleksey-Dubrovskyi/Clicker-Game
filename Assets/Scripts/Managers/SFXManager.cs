﻿using UnityEngine;

public enum Clip { Shoot, Teleport, Death, Click }

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    private AudioSource[] sfx;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    private void Start()
    {
        instance = this;
        sfx = GetComponents<AudioSource>();
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void PlaySFX(Clip audioClip)
    {
        sfx[(int)audioClip].Play();
    }
}
