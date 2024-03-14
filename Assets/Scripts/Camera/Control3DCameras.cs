using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control3DCameras : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            CameraManager.instance.Switch3DCameraDirection(0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            CameraManager.instance.Switch3DCameraDirection(1);
        }
    }
}
