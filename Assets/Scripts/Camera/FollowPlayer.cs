using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float offset2D;
    public float offset3D;
    public float offsetSmoothing;
    private Vector3 playerPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        float offset = Camera.main.orthographic ? offset2D : offset3D; //I need the inverted offset number in the 3d Mode
      
        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }
        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
      
    }

    
}
