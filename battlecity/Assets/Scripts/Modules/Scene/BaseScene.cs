public class BaseScene : BaseObject, IUpdate
{
    protected Command command = null;
    protected StateMachine FSM = null;    // FSM : ÓÐÏÞ×´Ì¬»ú(Finite-state Machine)
    protected InputHandler mInput = null;

    public void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate() { }

    public void ChangeState(StateBase mState)
    {
        FSM.CurrState = mState;
    }
}
