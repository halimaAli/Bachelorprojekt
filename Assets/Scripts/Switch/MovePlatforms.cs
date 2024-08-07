using UnityEngine;

public class MovePlatforms : MonoBehaviour
{

    private Vector3 originalPosition;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic)
        {
            transform.position = new Vector3(originalPosition.x, originalPosition.y, player.position.z);
            
            
        } else
        {

            transform.position = originalPosition;
            
        }
       
    }
}
