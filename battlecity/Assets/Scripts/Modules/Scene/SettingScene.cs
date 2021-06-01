public class SettingScene : BaseScene
{
    public SettingScene()
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
        FSM.CurrState = SettingState.Instance;
    }

    public override void OnUpdate()
    {
        command = mInput.HandleInput();
        if (null == command) { return; }
        command.OnExcute();

        base.OnUpdate();
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }
}
