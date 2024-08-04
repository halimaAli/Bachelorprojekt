using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float _startingPos;
    [SerializeField] float AmountOfParallax;

    private void Start()
    {
        _startingPos = transform.position.x;
    }

    private void Update()
    {
        Vector3 position = Camera.main.transform.position;
        float distance = position.x * AmountOfParallax;

        Vector3 newPosition = new Vector3(_startingPos + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;
    }
}
