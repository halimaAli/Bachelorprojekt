using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishIn3D : MonoBehaviour
{
    /*
     * Makes 2D Platforms vanish in 3D mode
     */

    private Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
       originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Camera.main.orthographic)
        {
            //transform.localPosition = new Vector3(originalPos.x, originalPos.y, 7.64f);
            gameObject.SetActive(true);
        } else
        {
            //transform.localPosition = originalPos;
            gameObject.SetActive(false);
        }
    }
}
