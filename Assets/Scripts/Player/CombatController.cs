using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CombatController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;


    // Update is called once per frame
    void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Camera.main.orthographic)
            {
                projectilePrefab.transform.rotation = Quaternion.Euler(0,0,0);
            }
            else
            {
                projectilePrefab.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }     
    }
}
