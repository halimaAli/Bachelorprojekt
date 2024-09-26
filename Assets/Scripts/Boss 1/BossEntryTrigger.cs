using UnityEngine;

public class BossEntryTrigger : MonoBehaviour
{
    private bool introPlayed = false;
    [SerializeField] private GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!introPlayed)
            {
                boss.SetActive(true);
                introPlayed = true;
            }     
        }
    }
}
