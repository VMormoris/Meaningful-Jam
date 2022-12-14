using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClips { Bumping=0, Cracking, Falling, Collecting, Rolling, Dying, Breaking}

public class SoundManagerScript : MonoBehaviour
{

    public AudioClip BumpingSound, CrackingSound, FallingSound, CollectingSound, RollingSound, DyingSound, BreakingSound;    
    // Start is called before the first frame update
    void Start()
    {
        if(GameContext.sSoundManager == null)
        {
            GameContext.sSoundManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlaySound(SoundClips clip)
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        switch (clip)
        {
            case SoundClips.Bumping:
                audioSrc.PlayOneShot(BumpingSound);
                break;
            case SoundClips.Cracking:
                audioSrc.PlayOneShot(CrackingSound);
                break;
            case SoundClips.Falling:
                audioSrc.PlayOneShot(FallingSound);
                break;
            case SoundClips.Collecting:
                audioSrc.PlayOneShot(CollectingSound);
                break;
            case SoundClips.Rolling:
                audioSrc.PlayOneShot(RollingSound);
                break;
            case SoundClips.Dying:
                audioSrc.PlayOneShot(DyingSound);
                break;
            case SoundClips.Breaking:
                Debug.Log("breaking");
                audioSrc.PlayOneShot(BreakingSound);
                break;

        }
    }

    public void StopSoundClip()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.Stop();
    }
}
