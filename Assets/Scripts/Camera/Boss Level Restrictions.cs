using UnityEngine;

public class BossLevelRestrictions : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void LateUpdate()
    {
        CalculateScreenBoundaries();
        Vector3 viewPos = transform.position;

        // Clamp x and y positions within the calculated boundaries
        viewPos.x = Mathf.Clamp(viewPos.x, minX, maxX);
       // viewPos.y = Mathf.Clamp(viewPos.y, minY, maxY);
        transform.position = viewPos;
    }

    void CalculateScreenBoundaries()
    {
        //Calculate minX and maxX based on viewports left and right edges
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + objectWidth;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - objectWidth;

        /*//Calculate minY and maxY based on viewports bottom and top edges
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + objectHeight;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - objectHeight;*/
    }
}
