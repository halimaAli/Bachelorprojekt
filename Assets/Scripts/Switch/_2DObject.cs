using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DObject : MonoBehaviour
{
    public int numberOfChildren;
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
        numberOfChildren = transform.childCount;
        for (int i = 0; i < numberOfChildren; i++)
        {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            if (obj != null)
            {
                obj.SetActive(active);
            }
            
        }
    }
}
