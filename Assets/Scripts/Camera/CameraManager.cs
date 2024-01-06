using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera _2DCamera;
    [SerializeField] private CinemachineVirtualCamera _3DCamera;
    private bool change = false;
    [SerializeField] private GameObject player;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            change = !change;    //Toggles between the modes
            if (change)
            {
                Set3DView();
            }
            else
            {
                Set2DView();
            }
        }
    }

    private void Set3DView()
    {
        Camera.main.orthographic = false;
        _2DCamera.Priority = 0;
        _3DCamera.Priority = 1;

        player.transform.eulerAngles -= new Vector3(0, -90, 0);
    }

    private void Set2DView()
    {
        Camera.main.orthographic = true;
        _2DCamera.Priority = 1;
        _3DCamera.Priority = 0;

        player.transform.eulerAngles -= new Vector3(0, 90, 0);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -8);
    }
}
