using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIKeyboardState : StateBase
{
    private static SettingUIKeyboardState mInstance = null;

    private SettingUIKeyboardState() { }

    public static SettingUIKeyboardState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIKeyboardState();
                mInstance.AddState(GameState.SETTING_UI_KEYBOARD);
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
        Debug.Log(string.Format("Enter \"SettingUIKeyboardState\"."));

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
