using UnityEngine;

public class VanishIn3D : MonoBehaviour
{
    /*
     * Makes 2D Objects vanish in 3D mode
     */
    void Update()
    {
        if (Camera.main.orthographic)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
