using UnityEngine;

public class MainMenuOnlineState : StateBase
{
    private static MainMenuOnlineState mInstance = null;

    private MainMenuOnlineState() { }

    public static MainMenuOnlineState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuOnlineState();
                mInstance.AddState(GameState.MAIN_MENU_ONLINE);
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
        Debug.Log(string.Format("Enter \"MainMenuOnlineState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_ONLINE_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.LOGIN); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectUp)) { command.OnExcute(GameState.MAIN_MENU_CUSTOMIZE); }
        if (command.GetType() == typeof(UISelectDown)) { command.OnExcute(GameState.MAIN_MENU_EXIT); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
