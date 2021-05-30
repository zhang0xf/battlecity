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

    public virtual void HandleUpCommand() { }

    public virtual void HandleDownCommand() { }

    public virtual void HandleLeftCommand() { }

    public virtual void HandleRightCommand() { }

    public virtual void HandleFireCommand() { }

    public virtual void HandleMouseMoveCommand() { }
}
