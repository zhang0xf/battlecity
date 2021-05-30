using UnityEngine;

public class MainMenuState : StateBase
{
    private static MainMenuState mInstance = null;

    private MainMenuState() 
    {
        Status = GameState.MENU;
    }

    public static MainMenuState Instance
    {
        get 
        {
            if (null == mInstance)
                mInstance = new MainMenuState();
            return mInstance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"MainMenuState\"."));
        UIManager.Instance.OpenUI(UIType.MAIN_MENU_UI);
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        Debug.Log(string.Format("Leave \"MainMenuState\"."));
        base.OnLeave();
    }
}
