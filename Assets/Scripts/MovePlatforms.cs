using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatforms : MonoBehaviour
{

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic)
        {
            transform.position = new Vector3(originalPosition.x, originalPosition.y, -8);
        } else
        {
            transform.position = originalPosition;
        }
       
    }
}
