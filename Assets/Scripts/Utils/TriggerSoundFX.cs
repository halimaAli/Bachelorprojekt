using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSoundFX : MonoBehaviour
{
    [SerializeField] private AudioClip FXSoundClip;
    [SerializeField] private bool isPlaying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (gameObject.tag.Equals("RespawnPoint"))
            {
                if (!isPlaying)
                {
                    SoundFXManager.instance.PlayLoopingSound(FXSoundClip, transform, 1);
                    isPlaying = true;
                }
            } else
            {
                SoundFXManager.instance.PlaySoundFXClip(FXSoundClip, transform, 1, false);
            }
        }
    }
}
