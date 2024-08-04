
using UnityEngine;

public class HangingShuriken : Enemy
{
    [SerializeField] private Direction movingDirection;
    private Vector3 dir;

    private enum Direction
    {
        Left,
        Right
    }

    public override void Start()
    {
        base.Start();
        if (movingDirection == Direction.Left)
        {
            dir = Vector3.left;
        } else if (movingDirection == Direction.Right)
        {
            dir = Vector3.right;
        }
    }

    protected override void Update()
    {
        if (!Camera.main.orthographic)
        {
            transform.Translate(dir * speed * direction * Time.deltaTime);
        }
    }


}
