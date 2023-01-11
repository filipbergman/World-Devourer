using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audioClipList;
    public AudioClip itemPopSound;

    private AudioSource audioSource;

    public void PlayPopSound()
    {
        StopAllCoroutines();
        StartCoroutine(PlaySoundEffect());
    }

    public void PlaySound(BlockType blockType)
    {
        StopAllCoroutines();
        StartCoroutine(PlaySoundEffect(blockType));
    }

    private IEnumerator PlaySoundEffect(BlockType blockType = BlockType.Air)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.SetParent(transform);
        Destroy(soundGameObject, 2);
        audioSource = soundGameObject.AddComponent<AudioSource>();
        if (blockType == BlockType.Air)
        {
            audioSource.PlayOneShot(itemPopSound);
            yield return null;
        }
            
        string blockName = blockType.ToString().ToLower();
        foreach (AudioClip clip in audioClipList)
        {
            if (clip.name.StartsWith(blockName))
                audioSource.PlayOneShot(clip);
        }
        yield return null;
    }
}