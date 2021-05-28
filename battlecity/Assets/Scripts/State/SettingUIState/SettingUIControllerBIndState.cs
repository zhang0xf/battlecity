using UnityEngine;

public class SettingUIControllerBIndState : StateBase
{
    private static SettingUIControllerBIndState mInstance = null;

    private SettingUIControllerBIndState() { }

    public static SettingUIControllerBIndState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIControllerBIndState();
                mInstance.AddState(GameState.SETTING_UI_CONTROLLERBIND);
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
        Debug.Log(string.Format("Enter \"SettingUIControllerBIndState\"."));

        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
