using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public AudioSource gameOverMusic;

    public AudioSource sfxCoin;
    public AudioSource sfxJJump;
    public AudioSource sfxHit;

    public bool soundMuted;

    public GameObject mutedImage;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SoundMuted"))
        {
            if(PlayerPrefs.GetInt("SoundMuted") == 1)
            {
                MuteAll();
                mutedImage.SetActive(true);
                soundMuted = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt("SoundMuted", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundOnOff()
    {
        if (soundMuted)
        {
            mutedImage.SetActive(false);
            soundMuted = false;
            UnmuteAll();
        }
        else
        {
            mutedImage.SetActive(true);
            soundMuted = true;
            MuteAll();
        }
    }

    public void MuteAll()
    {
        menuMusic.gameObject.SetActive(false);
        gameMusic.gameObject.SetActive(false);
        gameOverMusic.gameObject.SetActive(false);
        sfxCoin.gameObject.SetActive(false);
        sfxJJump.gameObject.SetActive(false);
        sfxHit.gameObject.SetActive(false);

        PlayerPrefs.SetInt("SoundMuted", 1);
    }

    public void UnmuteAll()
    {
        menuMusic.gameObject.SetActive(true);
        gameMusic.gameObject.SetActive(true);
        gameOverMusic.gameObject.SetActive(true);
        sfxCoin.gameObject.SetActive(true);
        sfxJJump.gameObject.SetActive(true);
        sfxHit.gameObject.SetActive(true);

        PlayerPrefs.SetInt("SoundMuted", 0);
    }

    public void StopMusic()
    {
        menuMusic.Stop();
        gameMusic.Stop();
        gameOverMusic.Stop();
    }
}
