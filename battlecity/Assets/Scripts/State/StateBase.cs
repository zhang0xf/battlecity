public class StateBase
{
    private GameState mStatus = GameState.NONE;

    public GameState Status
    {
        set { mStatus = value; }
        get { return mStatus; }
    }

    public virtual void OnEnter() { }

    public virtual void OnExcute() { }

    public virtual void OnLeave() { }
}