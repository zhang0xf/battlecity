public class MainMenuScene : BaseScene
{
    public MainMenuScene()
    {
        RegState = RegisterState.NONE;
    }

    protected override void OnLoad()
    {
        init();
        base.OnLoad();
    }

    private void init()
    {
        mInput = InputHandler.Instance;
        FSM = StateMachine.Instance(this);
        FSM.CurrState = MainMenuState.Instance;
    }

    public override void OnUpdate() 
    {
        command = mInput.HandleInput();
        handleCommand(command);

        base.OnUpdate();
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

    public void ChangeState(StateBase mState)
    {
        if (null == command || command.GetType() != typeof(FireCommand)) { return; }
        FSM.CurrState = mState;
    }

    public void handleCommand(Command command)
    {
        if (null == command) { return; }
        if (command.GetType() == typeof(UpCommand)) { HandleUpCommand(); }
        if (command.GetType() == typeof(DownCommand)) { HandleDownCommand(); }
        if (command.GetType() == typeof(LeftCommand)) { HandleLeftCommand(); }
        if (command.GetType() == typeof(RightCommand)) { HandleRightCommand(); }
        if (command.GetType() == typeof(FireCommand)) { HandleFireCommand(); }
        if (command.GetType() == typeof(MouseMoveCommand)) { HandleMouseMoveCommand(); }
    }

    public override void HandleUpCommand()
    {
        Notification na = new Notification(NotificationName.UI_SELECT_UP, this);
        na.Send();
        base.HandleUpCommand();
    }

    public override void HandleDownCommand()
    {
        Notification na = new Notification(NotificationName.UI_SELECT_DOWN, this);
        na.Send();
        base.HandleDownCommand();
    }

    public override void HandleLeftCommand()
    {
        Notification na = new Notification(NotificationName.UI_SELECT_LEFT, this);
        na.Send();
        base.HandleLeftCommand();
    }

    public override void HandleRightCommand()
    {
        Notification na = new Notification(NotificationName.UI_SELECT_RIGHT, this);
        na.Send();
        base.HandleRightCommand();
    }

    public override void HandleFireCommand()
    {
        Notification na = new Notification(NotificationName.FIRE, this);
        na.Send();
        base.HandleFireCommand();
    }

    public override void HandleMouseMoveCommand()
    {
        Notification na = new Notification(NotificationName.MOUSE_MOVE, this);
        na.Send();
        base.HandleMouseMoveCommand();
    }
}
