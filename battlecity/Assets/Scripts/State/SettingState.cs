using UnityEngine;

public class SettingState : StateBase
{
    private static SettingState mInstance = null;

    private SettingState()
    {
        Status = GameState.SETTING;
    }

    public static SettingState Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new SettingState();
            return mInstance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"SettingUIState\"."));
        UIManager.Instance.OpenUI(UIType.SETTING_UI);
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        Debug.Log(string.Format("Leave \"SettingUIState\"."));
        base.OnLeave();
    }
}
