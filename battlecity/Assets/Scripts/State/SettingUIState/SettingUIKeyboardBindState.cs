using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIKeyboardBindState : StateBase
{
    private static SettingUIKeyboardBindState mInstance = null;

    private SettingUIKeyboardBindState() { }

    public static SettingUIKeyboardBindState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIKeyboardBindState();
                mInstance.AddState(GameState.SETTING_UI_KEYBOARDBIND);
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
        Debug.Log(string.Format("Enter \"SettingUIKeyboardBindState\"."));

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
