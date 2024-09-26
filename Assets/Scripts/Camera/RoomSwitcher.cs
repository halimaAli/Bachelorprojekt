using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _2DCameraObj;
    [SerializeField] private CinemachineVirtualCamera _2DCamera;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool soundPlayed;


    private void OnTriggerEnter(Collider other)
    {
        if (!Camera.main.orthographic) return;
        if (other.CompareTag("Player"))
        {
            if (!soundPlayed)
            {
                SoundFXManager.instance.PlaySoundFXClip(_audioClip, transform, 1, false);
                soundPlayed = true;
            }

            _2DCameraObj.SetActive(true);
            CameraManager.instance.Set2DCamera(_2DCamera);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Camera.main.orthographic) return;
        if (other.CompareTag("Player"))
        {
            _2DCameraObj.SetActive(true);
            CameraManager.instance.Set2DCamera(_2DCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // if (!Camera.main.orthographic) return;
        if (other.CompareTag("Player"))
        {
            _2DCameraObj.SetActive(false);
        }
    }
}