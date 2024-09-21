using UnityEngine;

public class RotationController : MonoBehaviour
{
    private bool is2D;
    public float defaultZPos2D;

    // Start is called before the first frame update
    void Start()
    {
        is2D = true;
        defaultZPos2D = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Check the current view state of the game and rotate the gameobject accordingly
        if (Camera.main.orthographic && !is2D)
        {
            transform.eulerAngles -= new Vector3(0, 90, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, defaultZPos2D);
            is2D = true;
        } 
        else if (!Camera.main.orthographic && is2D)
        {
            transform.eulerAngles -= new Vector3(0, -90, 0);
            is2D = false;
        }
    }

    public void SetNewZPos2D(float zPos)
    {
        defaultZPos2D = zPos;
    }
}
