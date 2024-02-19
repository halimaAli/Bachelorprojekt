using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera _2DCamera;
    [SerializeField] private CinemachineVirtualCamera _3DCamera;
    public bool change = false;
    [SerializeField] private GameObject player;
    private GameObject[] enemies;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        // Find all enemies in the scene based on their tags
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))  // (Input.GetKeyUp(KeyCode.Q) && LevelManager.instance.canChangeView)
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
        UIHandler.instance.EnableText(false);

        player.transform.eulerAngles -= new Vector3(0, -90, 0);

        foreach (GameObject enemy in enemies)
        {
            enemy.transform.eulerAngles -= new Vector3(0, -90, 0);
        }
    }

    public void Set2DView()
    {
        Camera.main.orthographic = true;
        _2DCamera.Priority = 1;
        _3DCamera.Priority = 0;
        UIHandler.instance.EnableText(true);

        player.transform.eulerAngles -= new Vector3(0, 90, 0);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -8);
     
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.eulerAngles -= new Vector3(0, 90, 0);
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -8);
        }
    }
}
