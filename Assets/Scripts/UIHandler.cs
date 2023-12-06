using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private bool change  = false;
    public GameObject player;
    public Vector3 camera2DPos = new Vector3(1.05999994f, 1.57000005f, -12);

    public void OnChangeClick()
    {
        change = !change;
        if (change)
        {
            Camera.main.orthographic = false;
            Camera.main.transform.position = new Vector3(-22, -0.14f, -8);
            Camera.main.transform.eulerAngles -= new Vector3(5, -90, 0);
        }
        else
        {
            Camera.main.orthographic = true;
            Camera.main.transform.position = camera2DPos;
            Camera.main.transform.eulerAngles -= new Vector3(-5, 90, 0);
        }
    }

    public void onExitClick()
    { 
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit(); // original code to quit Unity player
    #endif
        
    }


}
