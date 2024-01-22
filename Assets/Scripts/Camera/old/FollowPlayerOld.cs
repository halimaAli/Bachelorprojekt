using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayerOld : MonoBehaviour
{
    public GameObject player;
    public float offset2D;
    public float offset3D;
    public float offsetSmoothing;
    private Vector3 playerPosition;
    public float speed;

    public Vector2 followOffset;
    private Vector2 treshold;


    private void Start()
    {
        treshold = calculateTreshold();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       /* currentPlayerPosition = player.transform.position;

        //distance between player and axis
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * currentPlayerPosition.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * currentPlayerPosition.y);

        Vector3 newPos = transform.position;

        //if player goes beyond treshould, adjust camera pos
        if (Mathf.Abs(xDifference) >= treshold.x)
        {
            newPos.x = currentPlayerPosition.x;
        }

        if (Mathf.Abs(yDifference) >= treshold.y)
        {
            newPos.y = currentPlayerPosition.y;
        }
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);*/


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

    /*private void FixedUpdate()
    {
        Vector2 follow = player.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPos = transform.position;

        if (Mathf.Abs(xDifference) >= treshold.x)
        {
            newPos.x = follow.x;
        }

        if (Mathf.Abs(yDifference) >= treshold.y)
        {
            newPos.y = follow.y;
        }

        transform.position = Vector3.MoveTowards(transform.position, newPos, speed*Time.deltaTime);

    }
*/
    private Vector3 calculateTreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width /  aspect.height, Camera.main.orthographicSize);

        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 border = calculateTreshold();
        Gizmos.DrawWireCube(transform.position,new Vector3( border.x*2, border.y*2,1));
    }


    */


}
