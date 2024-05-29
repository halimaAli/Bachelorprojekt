using System;
using UnityEngine;

public class DetectAndChase : Enemy
{
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] private DetectType detectType;
    private enum DetectType
    {
        inArea,
        inDistance
    }

    public override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
       // print("detect: " + detectedPlayer.ToString() + " attacking: " + attacking.ToString());
        base.Update();

        if (detectType == DetectType.inDistance)
        {
            DetectPlayer();
        }

        if (detectedPlayer && !attacking)
        {
            CheckAttack(attackRange);
            MoveTowardsPlayer();
            _animator.SetBool("isWalking", true);
        }
        else
        {
            bool atStartPos = ReturnToOriginalPosition();
            _animator.SetBool("isWalking", !atStartPos);
        }
    }

    private void DetectPlayer()
    {
        detectedPlayer = distance <= detectionRange;
    }

    public void DetectedInArea(bool detected)
    {
        detectedPlayer = detected;
    }
}
