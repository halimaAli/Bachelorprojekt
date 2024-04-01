using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera _2DCamera;
    [SerializeField] private CinemachineVirtualCamera[] _3DCameras;
    public bool change = false;
    private int _3DCameraIndex;
    internal CinemachineVirtualCamera currentCamera;

    private void Awake()
    {
        instance = this;
        _3DCameraIndex = 1;
        currentCamera = _2DCamera;
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

        StartCoroutine(Activate3DCameras());

        UIHandler.instance.EnableText(false);
    }

    public void Set2DView()
    {
        Camera.main.orthographic = true;
        _2DCamera.Priority = 1;
        _3DCameras[0].Priority = 0;
        _3DCameras[1].Priority = 0;
        currentCamera = _2DCamera;
        UIHandler.instance.EnableText(true);
    }

    public void Switch3DCameraDirection(int cameraIndex)
    {
        _3DCameraIndex = cameraIndex;

        StartCoroutine(Activate3DCameras());
    }

    private IEnumerator Activate3DCameras()
    {
        if (!Camera.main.orthographic) {
            for (int i = 0; i < _3DCameras.Length; i++)
            {
                if (i == _3DCameraIndex)
                {
                    _3DCameras[i].Priority = 1;
                }
                else
                {
                    _3DCameras[i].Priority = 0;
                }
            }
            PlayerController.instance.active = false; //TODO: make animations stop
            yield return new WaitForSeconds(1f);
            PlayerController.instance.active = true;


            currentCamera = _3DCameras[_3DCameraIndex];
        }
    }
}
