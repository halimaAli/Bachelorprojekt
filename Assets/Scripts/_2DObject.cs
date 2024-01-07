using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DObject : MonoBehaviour
{
    void Update()
    {
        //doesnt work because gameobject will be made inactive and updae wont be called
        if (Camera.main.orthographic)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
