
public abstract class BossBaseState : State
{
    protected BossStateController context;

    protected BossBaseState(BossStateController context)
    {
        this.context = context;
    }

    public virtual void FixedUpdate()
    {
       //noop
    }

    public virtual void Enter()
    {
        //noop
    }

    public virtual void Exit()
    {
        //noop
    }

    public virtual void UpdateState()
    {
        //noop
    }
}
