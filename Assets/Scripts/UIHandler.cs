using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private bool change  = false;
    public Vector3 camera2DPos = new Vector3(1.05999994f, 1.57000005f, -12);
    public float offset = 10;

    public void OnChangeClick()
    {
        change = !change; //vorläufig! unbedingt umändern
        if (change)
        {
            Camera.main.orthographic = false;

            Camera.main.transform.position = new Vector3(-22, -0.14f, -8);
            // Camera.main.transform.position = Vector3.Lerp(new Vector3(-22, -0.14f, -8),camera2DPos, offset*Time.deltaTime);
            Camera.main.transform.eulerAngles -= new Vector3(5, -90, 0);
           // Camera.main.transform.eulerAngles = Quaternion.Lerp(new Vector3(5, -90, 0), Camera.main.transform.rotation, offset * Time.deltaTime);
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
