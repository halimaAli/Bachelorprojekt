using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera _2DCamera;
    [SerializeField] private CinemachineVirtualCamera[] _3DCameras;

    private int _3DCameraIndex = 1;
    public CinemachineVirtualCamera currentCamera;

    // used in LevelManagers; decides if the player can control the view mode
    public bool allowViewModeChange = true;

    private void Awake()
    {
        instance = this;
        currentCamera = _2DCamera;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && allowViewModeChange)
        {
            if (currentCamera == _2DCamera)
            {
                Set3DView();
            }
            else
            {
                Set2DView();
            }
        }
    }

    public void Set3DView()
    {
        Camera.main.orthographic = false;
        _2DCamera.Priority = 0;
        StartCoroutine(Activate3DCameras());
    }

    public void Set2DView()
    {
        Camera.main.orthographic = true;
        _2DCamera.Priority = 1;
        foreach (var camera in _3DCameras)
        {
            camera.Priority = 0;
        }
        currentCamera = _2DCamera;
    }

    public void Switch3DCameraDirection(int cameraIndex)
    {
        _3DCameraIndex = cameraIndex;
        StartCoroutine(Activate3DCameras());
    }

    private IEnumerator Activate3DCameras()
    {
        if (!Camera.main.orthographic)
        {
            for (int i = 0; i < _3DCameras.Length; i++)
            {
                _3DCameras[i].Priority = (i == _3DCameraIndex) ? 1 : 0;
            }
            PlayerController.instance.active = false; // TODO: make animations stop
            yield return new WaitForSeconds(1f);
            PlayerController.instance.active = true;
            currentCamera = _3DCameras[_3DCameraIndex];
        }
    }
}
