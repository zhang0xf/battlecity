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
                mInstance.AddState(GameState.MIAN_MENU);
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
        base.OnEnter();
    }

    // BaseUI : OnUpdate() + ∂‡Ã¨
    public override void OnExcute()
    {
        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        UIType uIType = UIManager.Instance.GetPeekUIType();
        if (uIType != UIType.MAIN_MENU_UI) { return; }

        if (command.GetType() == typeof(UIComfirm))
        {
            ChangeState();
        }

        command.OnExcute();

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    public override void ChangeState()
    {
        GameObject uiObject = UIManager.Instance.GetPeekUI();
        if (null == uiObject) { return; }

        MainMenuUI mainMenuUI = uiObject.GetComponent<MainMenuUI>();
        if (null == mainMenuUI) { return; }

        if (mainMenuUI.IsOptionSettingSelected())
            StateMachine.Instance.ChangeState(GameState.SETTING_UI);
        else if (mainMenuUI.IsOptionPlayerSelected() || mainMenuUI.IsOptionPlayersSelected())
            StateMachine.Instance.ChangeState(GameState.START);
        else if (mainMenuUI.IsOptionOnlineSelected())
            StateMachine.Instance.ChangeState(GameState.LOGIN);
        else if (mainMenuUI.IsOptionExitSelected())
            StateMachine.Instance.ChangeState(GameState.EXIT);
    }
}
