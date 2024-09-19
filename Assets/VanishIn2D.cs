using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishIn2D : MonoBehaviour
{
    /*
    * Makes 3D Objects vanish in 2D mode
    */
    void Update()
    {
        if (!Camera.main.orthographic)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
