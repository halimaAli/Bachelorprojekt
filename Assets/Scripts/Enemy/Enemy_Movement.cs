using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public int speed;

    int direction = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * direction *  Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ground"))
        {
            direction *= -1;
        }
    }
}
