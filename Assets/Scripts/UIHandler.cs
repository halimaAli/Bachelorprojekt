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
    public GameObject player;

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
        Camera.main.transform.position = new Vector3(-22, -0.14f, -8);
        Camera.main.transform.eulerAngles -= new Vector3(5, -90, 0);
        player.transform.eulerAngles -= new Vector3(0, -90, 0);
    }

    private void Set2DView()
    {
        Camera.main.orthographic = true;
        Camera.main.transform.position = camera2DPos;
        Camera.main.transform.eulerAngles -= new Vector3(-5, 90, 0);
        player.transform.eulerAngles -= new Vector3(0, 90, 0);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -8);
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
