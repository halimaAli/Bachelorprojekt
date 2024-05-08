using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float _startingPos; // This is the starting position of the background.
    private float _lengthOfBackground; // This is the length of the whole background.
    public float AmountOfParallax; // This is amount of parallax scroll.
    public Camera MainCamera; // Reference of the camera.

    private void Start()
    {
        // Getting the starting X position of the background.
        _startingPos = transform.position.x;

        // Calculate the length of the whole background.
        _lengthOfBackground = 0f;
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                _lengthOfBackground += renderer.bounds.size.x;
            }
        }
    }

    private void Update()
    {
        Vector3 position = MainCamera.transform.position;
        float temp = position.x * (1 - AmountOfParallax);
        float distance = position.x * AmountOfParallax;

        Vector3 newPosition = new Vector3(_startingPos + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;

        if (temp > _startingPos + _lengthOfBackground / 2)
        {
            _startingPos += _lengthOfBackground;
        }
        else if (temp < _startingPos - _lengthOfBackground / 2)
        {
            _startingPos -= _lengthOfBackground;
        }
    }
}
