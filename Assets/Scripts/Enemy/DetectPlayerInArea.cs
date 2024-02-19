using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectPlayerInArea : MonoBehaviour
{
    [SerializeField]
    private MushroomController mushroomController;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag.Equals("Player"))
        {
            mushroomController.playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            mushroomController.playerDetected = false;
        }
    }
}
