
using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    [SerializeField] AudioSource walkingSound;

    public bool Walking { private get; set; }

    private void LateUpdate()
    {
        CheckAnimationState();
    }

    private void CheckAnimationState()
    {

        if (!walkingSound.isPlaying && Walking)
        {
            walkingSound.Play();
        }

        if (walkingSound.isPlaying && !Walking)
        {
            walkingSound.Stop();
        }
    }
}
