using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIControllerState : StateBase
{
    private static SettingUIControllerState mInstance = null;

    private SettingUIControllerState() { }

    public static SettingUIControllerState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIControllerState();
                mInstance.AddState(GameState.SETTING_UI_CONTROLLER);
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
        Debug.Log(string.Format("Enter \"SettingUIControllerState\"."));

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
