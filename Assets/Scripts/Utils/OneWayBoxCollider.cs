using System.Collections;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider OneWayTrigger;
    [SerializeField] private BoxCollider boxCollider;
    private bool playerIsOnPlatform;

    private void Start()
    {
        playerIsOnPlatform = false;
    }

    private void Update()
    {
        if (playerIsOnPlatform && Input.GetKeyDown(KeyCode.LeftShift))
        {
            print(gameObject.name);
            boxCollider.enabled = false;
            playerIsOnPlatform = false;
            StartCoroutine(EnableColldier());
        }
    }

    private IEnumerator EnableColldier()
    {
        yield return new WaitForSeconds(0.5f); //maybe a bit shorter duration
        boxCollider.enabled = true;
        playerIsOnPlatform = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Physics.IgnoreCollision(boxCollider, other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(boxCollider, other, false);
    }

    //checks if player is on the platform

    private void OnCollisionEnter(Collision collision)
    {
        playerIsOnPlatform = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        playerIsOnPlatform = false;
    }
}
