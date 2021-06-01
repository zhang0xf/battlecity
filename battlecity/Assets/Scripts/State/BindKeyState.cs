using UnityEngine;

public class BindKeyState : StateBase
{
    private static BindKeyState mInstance = null;

    private BindKeyState()
    {
        Status = GameState.BINDKEY;
    }

    public static BindKeyState Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new BindKeyState();
            return mInstance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"BindKeyState\"."));
        UIManager.Instance.OpenUI(UIType.BIND_KEY_UI);
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
