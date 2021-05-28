using UnityEngine;

public class MainMenuContinueState : StateBase
{
    private static MainMenuContinueState mInstance = null;

    private MainMenuContinueState() { }

    public static MainMenuContinueState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuContinueState();
                mInstance.AddState(GameState.MAIN_MENU_CONTINUE);
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
        Debug.Log(string.Format("Enter \"MainMenuContinueState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_CONTINUE_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.CONTINUE_GAME); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectUp)) { command.OnExcute(GameState.MAIN_MENU_NEW_GAME); }
        if (command.GetType() == typeof(UISelectDown)) { command.OnExcute(GameState.MAIN_MENU_SETTING); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
