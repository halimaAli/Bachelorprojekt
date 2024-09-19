using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeIntensity = 2f;
    [SerializeField] private float frequencyGain = 2f;

    private bool isShaking = false;
    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        _cbmcp = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        _cbmcp.m_AmplitudeGain =  shakeIntensity;
        _cbmcp.m_FrequencyGain = frequencyGain;

        isShaking = true;
    }

    public void StopShake()
    {
        _cbmcp.m_AmplitudeGain = 0f;
        _cbmcp.m_FrequencyGain = 0f;

        isShaking = false;
    }

    void Update()
    {
        if (BossStateController.instance == null)
        {
            return;
        }

        if (BossStateController.instance.introPlaying)
        {
            if (!isShaking)
            {
                ShakeCamera();
            }
        }
        else if (isShaking)
        {
            StopShake();
        }
    }
}
