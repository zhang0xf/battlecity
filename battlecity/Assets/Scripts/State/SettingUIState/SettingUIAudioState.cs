using UnityEngine;

public class SettingUIAudioState : StateBase
{
    private static SettingUIAudioState mInstance = null;

    private SettingUIAudioState() { }

    public static SettingUIAudioState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIAudioState();
                mInstance.AddState(GameState.SETTING_UI_AUDIO);
            }
            return mInstance;
        }
    }

    public override void AddState(GameState state)
    {
        base.AddState(state);
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"SettingUIAudioState\"."));

        base.OnEnter();
    }

    public override void OnExcute()
    {
        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        UIType uIType = UIManager.Instance.GetPeekUIType();
        if (uIType != UIType.SETTING_UI) { return; }

        if (command.GetType() == typeof(UIBack))
            StateMachine.Instance.ChangeState(GameState.MAIN_MENU);
        else if (command.GetType() == typeof(UISelectLeft))
        {

        }
        else if (command.GetType() == typeof(UISelectRight))
        {

        }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
