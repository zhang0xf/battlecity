using UnityEngine;

public class MainMenuCustomizeState : StateBase
{
    private static MainMenuCustomizeState mInstance = null;

    private MainMenuCustomizeState() { }

    public static MainMenuCustomizeState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuCustomizeState();
                mInstance.AddState(GameState.MAIN_MENU_CUSTOMIZE);
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
        Debug.Log(string.Format("Enter \"MainMenuCustomizeState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_CUSTOMIZE_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.CUSTOMIZE_GAME); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectUp)) { command.OnExcute(GameState.MAIN_MENU_SETTING); }
        if (command.GetType() == typeof(UISelectDown)) { command.OnExcute(GameState.MAIN_MENU_ONLINE); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
