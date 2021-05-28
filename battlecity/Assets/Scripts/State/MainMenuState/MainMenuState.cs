using UnityEngine;

public class MainMenuState : StateBase
{
    private static MainMenuState mInstance = null;

    private MainMenuState() { }

    public static MainMenuState Instance
    {
        get 
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuState();
                mInstance.AddState(GameState.MAIN_MENU);
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
        Debug.Log(string.Format("Enter \"MainMenuState\"."));
        UIManager.Instance.OpenUI(UIType.MAIN_MENU_UI);
        // OpenUI()包含协程且打开UI时需要注册消息，不能立刻切换状态。UI为READY状态再切换。
        base.OnEnter();
    }

    public override void OnExcute()
    {
        if (!IsUIReady(UIType.MAIN_MENU_UI)) { return; }

        StateMachine.Instance.ChangeState(GameState.MAIN_MENU_NEW_GAME);

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
