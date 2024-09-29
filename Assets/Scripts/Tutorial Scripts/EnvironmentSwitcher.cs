using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject bright2DBackground;
    [SerializeField] private GameObject dark2DBackground;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            bright2DBackground.SetActive(false);
            dark2DBackground.SetActive(true);
        } 

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
