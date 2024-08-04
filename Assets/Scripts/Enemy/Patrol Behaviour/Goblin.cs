using UnityEngine;

public class Goblin : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Move();
    }
}
