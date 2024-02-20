using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float attackDistance = 2;
    [SerializeField] float threshold = .4f;

    public void EnemyAttackAnimationEnded()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackDistance + threshold)
        {
            //Kill Player if Attack-Animation ended and the player is still close to enemy
            PlayerController.instance.Die(false);
        }
        //set the isAttacking flag to false when needed
    }
}
