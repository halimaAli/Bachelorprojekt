using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwayToPlayer : MonoBehaviour
{
    public CinemachineVirtualCamera initialCamera;
    public CinemachineVirtualCamera followCamera;
    public float blendTime = 2f;

    void Start()
    {
        initialCamera.Priority = 10;
        followCamera.Priority = 0;

        StartCoroutine(SwitchToFollowCamera());
    }

    private IEnumerator SwitchToFollowCamera()
    {
        PlayerController.instance.active = false;
        yield return new WaitForSeconds(blendTime);

        initialCamera.Priority = 0;
        followCamera.Priority = 10;
        yield return new WaitForSeconds(6f);
        PlayerController.instance.active = true;
    }

}
