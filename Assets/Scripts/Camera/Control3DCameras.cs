using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control3DCameras : MonoBehaviour
{
    [SerializeField] private Direction direction;

    private enum Direction
    {
        Backwards,
        Forwards
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (direction == Direction.Backwards && !CameraManager.instance.isBackwards3D)
            {
                CameraManager.instance.Switch3DCameraDirection(0);
                CameraManager.instance.isBackwards3D = true;
            }
            else if (direction == Direction.Forwards && CameraManager.instance.isBackwards3D)
            {
                CameraManager.instance.Switch3DCameraDirection(1);
                CameraManager.instance.isBackwards3D = false;
            }
        }
    }
}
