using UnityEngine;

public class MainMenuExitState : StateBase
{
    private static MainMenuExitState mInstance = null;

    private MainMenuExitState() { }

    public static MainMenuExitState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuExitState();
                mInstance.AddState(GameState.MAIN_MENU_EXIT);
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
        Debug.Log(string.Format("Enter \"MainMenuExitState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_EXIT_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectUp)) { command.OnExcute(GameState.MAIN_MENU_ONLINE); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
