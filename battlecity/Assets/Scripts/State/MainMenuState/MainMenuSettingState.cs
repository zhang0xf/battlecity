using UnityEngine;

public class MainMenuSettingState : StateBase
{
    private static MainMenuSettingState mInstance = null;

    private MainMenuSettingState() { }

    public static MainMenuSettingState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuSettingState();
                mInstance.AddState(GameState.MAIN_MENU_SETTING);
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
        Debug.Log(string.Format("Enter \"MainMenuSettingState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_SETTING_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.SETTING_UI); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectUp)) { command.OnExcute(GameState.MAIN_MENU_CONTINUE); }
        if (command.GetType() == typeof(UISelectDown)) { command.OnExcute(GameState.MAIN_MENU_CUSTOMIZE); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
