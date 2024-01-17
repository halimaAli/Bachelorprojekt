using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DObject : MonoBehaviour
{
    [SerializeField] int numberOfChildren;
    void Update()
    {
        if (Camera.main.orthographic)
        {
            ToggleVisibility(true); 
        }
        else
        {
            ToggleVisibility(false);
        }
    }

    void ToggleVisibility(bool active)
    {
        for (int i = 0; i < numberOfChildren; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
