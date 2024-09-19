using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackState : MonoBehaviour
{
    public GameObject[] slashFX = new GameObject[3];
    public Transform[] comboPos = new Transform[3];
    public AudioClip[] audioClips = new AudioClip[3];

    public void ComboAttackPhases(int phase)
    {
        SoundFXManager.instance.PlaySoundFXClip(audioClips[phase], transform, 1, false);
        slashFX[phase].transform.localScale = comboPos[phase].transform.localScale;
        Instantiate(slashFX[phase], comboPos[phase].position, slashFX[phase].transform.rotation);
    }
}
