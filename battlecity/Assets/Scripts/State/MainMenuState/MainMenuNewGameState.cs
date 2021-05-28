using UnityEngine;

public class MainMenuNewGameState : StateBase
{
    private static MainMenuNewGameState mInstance = null;

    private MainMenuNewGameState() { }

    public static MainMenuNewGameState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuNewGameState();
                mInstance.AddState(GameState.MAIN_MENU_NEW_GAME);
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
        Debug.Log(string.Format("Enter \"MainMenuNewGameState\"."));
        Notification na = new Notification(NotificationName.UI_SELECT_NEWGAME_OPTION, this);
        na.Send();
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        if (command.GetType() == typeof(UIComfirm)) { command.OnExcute(GameState.NEW_GAME); }
        if (command.GetType() == typeof(UIBack)) { command.OnExcute(GameState.EXIT); }
        if (command.GetType() == typeof(UISelectDown)) { command.OnExcute(GameState.MAIN_MENU_CONTINUE); }

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
