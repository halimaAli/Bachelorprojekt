using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject bg2D;
    [SerializeField] private GameObject bg3D;
    [SerializeField] private GameObject bushes3D;

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic)
        {
            bg2D.SetActive(true);
            bg3D.SetActive(false);
            bushes3D.SetActive(false);
        } else
        {
            bg2D.SetActive(false);
            bg3D.SetActive(true);
            bushes3D.SetActive(true);
        }
    }
}
