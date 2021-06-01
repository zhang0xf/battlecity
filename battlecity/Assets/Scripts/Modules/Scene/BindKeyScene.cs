using UnityEngine;

public class BindKeyScene : BaseScene
{
    public BindKeyScene()
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
        FSM.CurrState = BindKeyState.Instance;
    }

    public override void OnUpdate()
    {
        KeyCode keycode = mInput.GetKeyCodeDown();
        if (keycode == KeyCode.None) { return; }
        Notification na = new Notification(NotificationName.BIND_KEY, this);
        na.Content = keycode;
        na.Send();

        base.OnUpdate();
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }
}
