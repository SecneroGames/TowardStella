using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    //public static AudioManager Instance;
    [System.Serializable]
    public class AudioSet
    {
        public string clipID;
        public AudioClip clip;
    }

    //singleton
    public static AudioManager instance;

    public List<AudioSet> AudioClips = new List<AudioSet>();
    public AudioSource SFX_AudioSource;
    public AudioSource BGM_AudioSource;

    public AudioMixer mixer;
    private void Awake()
    {
        if(AudioManager.instance!=null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    public void PlayBGM(string clipID) 
    {
        clipID = clipID.TrimEnd();// remove spaces of string just in case

        //search for id
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if (AudioClips[i].clipID == clipID)
            {
                BGM_AudioSource.clip = AudioClips[i].clip;
                BGM_AudioSource.Play();
                Debug.Log($"PlaySFX {clipID}");
                return; // exit when clip is found
            }
        }
        Debug.LogError($"{clipID} - Invalid ID");

    }

    public void PlayBGM(int clipIndex)
    {
        try
        {
            BGM_AudioSource.clip = AudioClips[clipIndex].clip;
            BGM_AudioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void PlayBGMOnce(string clipID)
    {
        clipID = clipID.TrimEnd();// remove spaces of string just in case

        //search for id
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if (AudioClips[i].clipID == clipID)
            {
                BGM_AudioSource.PlayOneShot(AudioClips[i].clip);
                Debug.Log($"PlaySFX {clipID}");
                return; // exit when clip is found
            }
        }
        Debug.LogError($"{clipID} - Invalid ID");

    }

    public void PlaySFX(string clipID)
    {
        clipID = clipID.TrimEnd();// remove spaces of string just in case

        //search for id
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if (AudioClips[i].clipID == clipID)
            {                
                SFX_AudioSource.PlayOneShot(AudioClips[i].clip);
                Debug.Log($"PlaySFX {clipID}");
                return; // exit when clip is found
            }
        }
        Debug.LogError($"{clipID} - Invalid ID");

    }

    public void PlaySFX(int clipIndex)
    {
        try
        {
            SFX_AudioSource.PlayOneShot(AudioClips[clipIndex].clip);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

     public void PlaySFX(AudioSource source,string clipID)
    {
        clipID = clipID.TrimEnd();// remove spaces of string just in case

        //search for id
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if (AudioClips[i].clipID == clipID)
            {
                source.PlayOneShot(AudioClips[i].clip);
                Debug.Log($"PlaySFX {clipID}");
                return; // exit when clip is found
            }
        }
        Debug.LogError($"{clipID} - Invalid ID");

    }

  
}
