using UnityEngine;

public class TriggerHint : MonoBehaviour
{ 
    public float hintDelay = 3f;      
    private bool playerInPit = false; 
    private float timeInPit = 0f;
    [SerializeField] string message;
    [SerializeField] string key;

    private HintManager hintManager;
    private bool hintShowing;
    private bool shown;

    private void Start()
    {
        hintManager = FindObjectOfType<HintManager>();
    }

    private void Update()
    {
        if (playerInPit)
        {
            timeInPit += Time.deltaTime;

            if (timeInPit >= hintDelay && !hintShowing && !shown)
            {
                hintShowing = true;
                shown = true;
                hintManager.ShowHint(message, key);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPit = true;
            timeInPit = 0f; // Reset the timer
            hintShowing = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPit = false;
            if (hintShowing)
            {
                hintManager.HideHint();

            }
        }
    }
}
