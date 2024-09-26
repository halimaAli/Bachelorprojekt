using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject bg2D;
    [SerializeField] private GameObject bg3D;
    [SerializeField] private GameObject decoration3D;

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic)
        {
            if (bg2D != null) bg2D.SetActive(true);
            bg3D.SetActive(false);
            if (decoration3D != null) { decoration3D.SetActive(false); }
        } 
        else
        {
            if (bg2D != null) bg2D.SetActive(false);
            bg3D.SetActive(true);
            if (decoration3D != null) { decoration3D.SetActive(true); }
        }
    }
}
