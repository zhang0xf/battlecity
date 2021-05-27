using UnityEngine;

public class SettingUIState : StateBase
{
    private static SettingUIState mInstance = null;

    private SettingUIState()
    { 
        
    }

    public static SettingUIState Instance
    {
        get
        {
            if (null == mInstance)
            {
                mInstance = new SettingUIState();
                mInstance.AddState(GameState.SETTING_UI);
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
        base.OnLeave();
    }

    public override void ChangeState()
    {
        base.ChangeState();
    }



}
