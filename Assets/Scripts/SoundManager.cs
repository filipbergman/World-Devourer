using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audioClipList;

    public AudioClip itemPopSound;

    public void PlayPopSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(itemPopSound);
    }

    public void PlaySound(BlockType blockType)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        string blockName = blockType.ToString().ToLower();
        foreach (AudioClip clip in audioClipList)
        {
            if (clip.name.StartsWith(blockName))
                audioSource.PlayOneShot(clip);
        }
    }
}
