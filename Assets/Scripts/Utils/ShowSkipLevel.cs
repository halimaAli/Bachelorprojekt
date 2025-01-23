
using UnityEngine;

public class ShowSkipLevel : MonoBehaviour
{
    private bool enterAllowed;
    [SerializeField] private GameObject toolTip;
    [SerializeField] private GameObject skipLevelPopUp;
    private bool entered;

    private void Start()
    {
        skipLevelPopUp.SetActive(false);
        if (toolTip != null)
        {
            toolTip.SetActive(false);
        }
    }

    private void Update()
    {
        if (enterAllowed && Input.GetKeyDown(KeyCode.Return))
        {
            PlayerController.instance.active = false;
            skipLevelPopUp.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (toolTip != null)
            {
                toolTip.SetActive(true);
            }
            enterAllowed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (toolTip != null)
            {
                toolTip.SetActive(false);
            }
            enterAllowed = true;
        }
    }
}
