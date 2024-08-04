using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public bool canOpen;
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        canOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 2);
        }
    }

    public void Open()
    {
        canOpen = true;   
    }
}
